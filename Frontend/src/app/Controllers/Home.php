<?php

namespace App\Controllers;

use App\Filters\CIAuth;
use App\Models\RoomModel;


class Home extends BaseController
{
    private RoomModel $roomModel;
    private CIAuth $CIAuth;

    public function __construct()
    {
        $this->CIAuth = new CIAuth();
        $this->roomModel = new RoomModel();
    }

    public function index(): string
    {
        $rooms = $this->roomModel->getRooms();
        $timeSeries = $this->roomModel->getTemperatureData();

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
            $categories[] = $time->format('Y-m-d H:i');
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
}
