<?php

namespace App\Models;

use App\Entities\Room;
use App\Filters\CIAuth;
use DateTime;

class RoomModel
{
    private CIAuth $CIAuth;

    public function __construct()

    {
        $this->CIAuth = new CIAuth();
    }

    /**
     * @return Room[]
     */
    public function getRooms(): array
    {
        try {
            $client = \Config\Services::curlrequest();

            $response = $client->request('GET', 'http://mainunit:8080/api/rooms?skip=0&limit=100', [
                'headers' => [
                    'authorization' => 'Bearer ' . $this->CIAuth->user()->getToken(),
                ],
            ]);
            $rooms = [];
            foreach (json_decode($response->getBody()) as $room) {
                $rooms[$room->id] = new Room(
                    $room->id,
                    $room->name,
                    (float)$room->temperature,
                    $room->thermostatIds);
            }
            return $rooms;

        } catch (\Exception $e) {
            return [];
        }
    }

    public function getRoom(string $id): Room
    {
        try {
            $client = \Config\Services::curlrequest();

            $response = $client->request('GET', 'http://mainunit:8080/api/rooms/' . $id, [
                'headers' => [
                    'authorization' => 'Bearer ' . $this->CIAuth->user()->getToken(),
                ],
            ]);
            $roomResponse = json_decode($response->getBody());
            return new Room(
                $roomResponse->id,
                $roomResponse->name,
                (float)$roomResponse->temperature,
                $roomResponse->thermostatIds);
        } catch (\Exception $e) {
            throw new \Exception('Room not found');
        }
    }

    public function setRoomTemperature(string $id, float $temperature): bool
    {
        try {
            $client = \Config\Services::curlrequest();

            $response = $client->request('PUT', 'http://mainunit:8080/api/rooms/' . $id . '/temperature', [
                'headers' => [
                    'content-type' => 'application/json',
                    'authorization' => 'Bearer ' . $this->CIAuth->user()->getToken(),
                ],
                'json' => $temperature
            ]);

            return true;

        } catch (\Exception $e) {
            return false;
        }
    }

    public function createRoom(string $name): bool
    {
        try {
            $client = \Config\Services::curlrequest();

            $response = $client->request('POST', 'http://mainunit:8080/api/rooms', [
                'headers' => [
                    'authorization' => 'Bearer ' . $this->CIAuth->user()->getToken(),
                ],
                'json' => [
                    'name' => $name,
                    'description' => ''
                ]
            ]);
        } catch (\Exception $e) {
            return false;
        }
        return true;
    }

    public function updateRoom(string $id, string $name, float $temperature): bool
    {
        try {
            $client = \Config\Services::curlrequest();
            $response = $client->request('PUT', 'http://mainunit:8080/api/rooms', [
                'headers' => [
                    'authorization' => 'Bearer ' . $this->CIAuth->user()->getToken(),
                ],
                'json' => [
                    'id' => $id,
                    'name' => $name,
                    'description' => '',
                ]
            ]);
        } catch (\Exception $e) {
            return false;
        }

        try {
            $this->setRoomTemperature($id, $temperature);
        } catch (\Exception $e) {
            return false;
        }
        return true;
    }

    public function getTemperatureData()
    {
        $currentDateTime = new DateTime('tomorrow');
        $currentDate = $currentDateTime->format('Y-m-d');
        try {
            $client = \Config\Services::curlrequest();
            $response = $client->request('GET', 'http://mainunit:8080/api/RoomTemperature?start=2022-01-01&end=' . $currentDate, [
                'headers' => [
                    'authorization' => 'Bearer ' . $this->CIAuth->user()->getToken(),
                ],
            ]);
        } catch (\Exception $e) {
            return [];
        }
    }
}