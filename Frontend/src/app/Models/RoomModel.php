<?php

namespace App\Models;

use App\Entities\Room;

class RoomModel
{
    public function __construct()
    {
    }

    /**
     * @return Room[]
     */
    public function getRooms(): array {
        try {
            $client = \Config\Services::curlrequest();

            $response = $client->request('GET', 'http://mainunit:8080/api/rooms?skip=0&limit=100');
            $rooms = [];
            foreach (json_decode($response->getBody()) as $room) {
                $rooms[] = new Room(
                    $room->id,
                    $room->name,
                    (float) $room->temperature,
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

            $response = $client->request('GET', 'http://mainunit:8080/api/rooms/' . $id);
            $room_response = json_decode($response->getBody());
            return new Room(
                $room_response->id,
                $room_response->name,
                (float) $room_response->temperature,
                $room_response->thermostatIds);
        } catch (\Exception $e) {
            throw new \Exception('Room not found');
        }
    }

    public function setRoomTemperature(int $id, float $temperature): bool
    {
        return true;
    }

    public function createRoom(string $name): bool
    {
        try {
            $client = \Config\Services::curlrequest();

            $response = $client->request('POST', 'http://mainunit:8080/api/rooms', [
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

    public function updateRoom(string $id, int $param, float $param1): bool {
        return true;
    }

}