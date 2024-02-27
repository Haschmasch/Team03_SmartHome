<?php

namespace App\Models;

use App\Entities\Room;

class RoomModel
{
    public function __construct()
    {
    }

    public function getRooms(): array
    {
        return [
            '1' => new Room(1, 'Schlafzimmer', 18, []),
            '2' => new Room(2, 'Küche', 5, []),
            '3' => new Room(3, 'Kinderzimmer', 30, []),
            '4' => new Room(4, 'Wohnzimmer', 25, []),
        ];
    }

    public function getRoom(int $id): Room
    {
        switch ($id) {
            case 2:
                return new Room(2, 'Küche', 5, []);
            case 3:
                return new Room(3, 'Kinderzimmer', 30, []);
            case 4:
                return new Room(4, 'Wohnzimmer', 25, []);
            default:
                return new Room(1, 'Schlafzimmer', 18, []);
        }
    }

    public function setRoomTemperature(int $id, float $temperature): void
    {

    }

    public function createRoom(string $name): void
    {
        $client = \Config\Services::curlrequest();

        $response = $client->request('POST', 'http://mainunit:8080/api/rooms', [
            'json' => [
                'name' => $name,
                'description' => ''
            ]
        ]);
        var_dump($response);

        die();
    }
}