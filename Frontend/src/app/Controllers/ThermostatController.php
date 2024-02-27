<?php

namespace App\Controllers;


use App\Entities\Room;
use App\Models\RoomModel;
use App\Models\ThermostatModel;
use CodeIgniter\HTTP\RedirectResponse;

class ThermostatController extends BaseController
{
    private ThermostatModel $thermostatModel;
    private RoomModel $roomModel;

    public function __construct()
    {
        $this->thermostatModel = new ThermostatModel();
        $this->roomModel = new RoomModel();
    }

    public function index(): string
    {
        return view('pages/thermostat/thermostats', [
            'pageTitle' => 'Thermostate Übersicht',
            'thermostats' => $this->thermostatModel->getThermostats(),
            'rooms' => $this->roomModel->getRooms(),
            'user' => [
                'name' => 'Max Mustermann',
                'email' => 'max@mustermann.de',
            ],
        ]);
    }

    public function edit(string $id): string|RedirectResponse
    {
        $roomIDs = implode(',', array_map(fn(Room $room) => $room->getID(), $this->roomModel->getRooms()));
        $viewData = [
            'pageTitle' => 'Thermostat bearbeiten',
            'thermostat' => $this->thermostatModel->getThermostat($id),
            'rooms' => $this->roomModel->getRooms(),
        ];
        if (!empty($_POST)) {
            $isValid = $this->validate([
                'room' => [
                    'rules' => 'required|in_list[0,' . $roomIDs . ']',
                    'errors' => [
                        'required' => 'Bitte gib ein Zimmer an',
                        'in_list' => 'Bitte gib ein gültiges Zimmer an'
                    ]
                ],
            ]);

            if (!$isValid) {
                $viewData['validation'] = $this->validator;
                return view('pages/thermostat/edit', $viewData);
            } else {
                $roomSet = $this->thermostatModel->setRoom($id, (int)$this->request->getPost('room'));

                if (!$roomSet) {
                    return redirect()->route('thermostat.edit', [$id])->with('fail', 'Thermostat konnte dem Raum nicht zugewisesen werden')->withInput();
                } else {
                    return redirect()->route('thermostats')->with('success', 'Thermostat erfolgreich bearbeitet');
                }
            }

        }

        return view('pages/thermostat/edit', $viewData);
    }
}