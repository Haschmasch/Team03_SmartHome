<?php

namespace App\Controllers;

use App\Filters\CIAuth;
use App\Models\RoomModel;
use App\Models\ThermostatModel;


class Home extends BaseController
{
    private RoomModel $roomModel;
    private CIAuth $CIAuth;
    private ThermostatModel $thermostatModel;

    public function __construct()
    {
        $this->CIAuth = new CIAuth();
        $this->roomModel = new RoomModel();
        $this->thermostatModel = new ThermostatModel();
    }

    public function index(): string
    {
        $rooms = $this->roomModel->getRooms();
        $timeSeries = $this->roomModel->getTemperatureData();

        if (empty($timeSeries)) {
            return view('pages/home', [
                'pageTitle' => 'Dashboard',
                'rooms' => $rooms,
                'user' => $this->CIAuth->user(),
                'graphData' => []
            ]);
        }

        // Assuming $timeSeries is an array of objects with temperature measurements
        $times = [];
        $temperaturesByRoom = [];

        foreach ($timeSeries as $temperature) {
            $time = new \DateTime($temperature->timestamp);
            $times[] = $time;
            $temperaturesByRoom[$temperature->metadata->roomId][] = [
                'temperature' => $temperature->temperature,
                'time' => $time
            ];
        }
        foreach ($times as $time) {
            $categories[] = '';
        }

        // Step 1: Sort $times to ensure chronological order
        usort($times, function ($a, $b) {
            return $a <=> $b;
        });
        // Step 2: Normalize the temperature measurements
        $series = [];
        foreach ($temperaturesByRoom as $roomId => $roomTemperatures) {
            // Initialize the last known temperature for each room
            $lastKnownTemperature = null;

            foreach ($times as $time) {
                // Check if this room has a measurement at this time
                $found = false;
                foreach ($roomTemperatures as $measurement) {
                    if ($measurement['time']->format('Y-m-d H:i:s') === $time->format('Y-m-d H:i:s')) {
                        $series[$roomId][] = $measurement['temperature'];
                        $lastKnownTemperature = $measurement['temperature'];
                        $found = true;
                        break;
                    }
                }

                // If no measurement found for this time, use the last known temperature
                if (!$found && $lastKnownTemperature !== null) {
                    $series[$roomId][] = $lastKnownTemperature;
                }
            }
        }
        $graphData = [];
        foreach ($series as $roomId => $temperatures) {
            $graphData['series'][] = [
                "name" => $rooms[$roomId]->getName(),
                "data" => $temperatures
            ];
        }
        $graphData['categories'] = $categories;

        return view('pages/home', [
            'pageTitle' => 'Dashboard',
            'rooms' => $rooms,
            'user' => $this->CIAuth->user(),
            'graphData' => $graphData
        ]);
    }

    public function createDummyData()
    {

        $this->roomModel->createRoom('Küche');
        $this->roomModel->createRoom('Schlafzimmer');

        $thermostats = $this->thermostatModel->getThermostats();
        $rooms = $this->roomModel->getRooms();

        $roomIds = [];

        $i = 0;
        foreach ($rooms as $room) {
            $this->thermostatModel->setRoom($thermostats[$i]->getID(), $room->getID());
            $roomIds[] = $room->getID();
            $i++;
        }


        $this->roomModel->setRoomTemperature($roomIds[0], 16);
        $this->roomModel->setRoomTemperature($roomIds[1], 20);


        return redirect()->to(base_url(route_to('home')));

    }
}
