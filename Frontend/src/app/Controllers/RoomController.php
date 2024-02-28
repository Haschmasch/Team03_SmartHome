<?php

namespace App\Controllers;


use App\Entities\Room;
use App\Filters\CIAuth;
use App\Models\RoomModel;
use App\Models\ThermostatModel;
use CodeIgniter\HTTP\RedirectResponse;

class RoomController extends BaseController
{
    private RoomModel $roomModel;
    private ThermostatModel $thermostatModel;
    private CIAuth $CIAuth;

    public function __construct(

    )
    {
        $this->roomModel = new RoomModel();
        $this->thermostatModel = new ThermostatModel();
        $this->CIAuth = new CIAuth();
    }

    public function index(): string
    {
        return view('pages/thermostat/thermostats', [
            'pageTitle' => 'Thermostate Übersicht',
            'thermostats' => $this->thermostatModel->getThermostats(),
            'rooms' => $this->roomModel->getRooms(),
            'user' => $this->CIAuth->user()
        ]);
    }

    public function edit(string $id): string|RedirectResponse
    {
        $viewData = [
            'pageTitle' => 'Raum bearbeiten',
            'room' => $this->roomModel->getRoom($id),
        ];
        if (!empty($_POST)) {
            $isValid = $this->validate([
                'name' => [
                    'rules' => 'required|min_length[3]',
                    'errors' => [
                        'required' => 'Bitte gib einen Namen für den Raum an',
                        'min_length' => 'Der Name des Raums muss mindestens 3 Zeichen lang sein'
                    ]
                ],
                'temperature' => [
                    'rules' => 'required|numeric',
                    'errors' => [
                        'required' => 'Bitte gib eine Temperatur für den Raum an',
                        'numeric' => 'Die Temperatur muss eine Zahl sein'
                    ]
                ],
            ]);

            if (!$isValid) {
                $viewData['validation'] = $this->validator;
                return view('pages/room/edit', $viewData);
            } else {
                $roomSet = $this->roomModel->updateRoom(
                    $id,
                    $this->request->getPost('name'),
                    (float)$this->request->getPost('temperature'));

                if (!$roomSet) {
                    return redirect()->route('room.edit', [$id])->with('fail', 'Raum konnte dem Raum nicht bearbeitet werden')->withInput();
                } else {
                    return redirect()->route('home')->with('success', 'Raum erfolgreich bearbeitet');
                }
            }

        }

        return view('pages/room/edit', $viewData);
    }

    public function create(): string|RedirectResponse
    {
        $viewData = [
            'pageTitle' => 'Raum hinzufügen',
        ];
        if (!empty($_POST)) {
            $isValid = $this->validate([
                'name' => [
                    'rules' => 'required|min_length[3]',
                    'errors' => [
                        'required' => 'Bitte gib einen Namen für den Raum an',
                        'min_length' => 'Der Name des Raums muss mindestens 3 Zeichen lang sein'
                    ]
                ],
            ]);

            if (!$isValid) {
                $viewData['validation'] = $this->validator;
                return view('pages/room/create', $viewData);
            } else {
                $roomCreated = $this->roomModel->createRoom($this->request->getPost('name'));

                if (!$roomCreated) {
                    return redirect()->route('room.create')->with('fail', 'Der Raum könnte nicht hinzugefügt werden')->withInput();
                } else {
                    return redirect()->route('home')->with('success', 'Raum erfolgreich erzeugt');
                }
            }
        }

        return view('pages/room/create', $viewData);

    }
}
