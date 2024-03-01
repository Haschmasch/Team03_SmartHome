<?php

namespace App\Models;

use App\Entities\Room;
use DateTime;

class RoomModel
{
    public function __construct()
    {
    }

    /**
     * @return Room[]
     */
    public function getRooms(): array
    {
        try {
            $client = \Config\Services::curlrequest();

            $response = $client->request('GET', 'http://mainunit:8080/api/rooms?skip=0&limit=100');
            $rooms = [];
            foreach (json_decode($response->getBody()) as $room) {
                $rooms[] = new Room(
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

            $response = $client->request('GET', 'http://mainunit:8080/api/rooms/' . $id);
            $room_response = json_decode($response->getBody());
            return new Room(
                $room_response->id,
                $room_response->name,
                (float)$room_response->temperature,
                $room_response->thermostatIds);
        } catch (\Exception $e) {
            throw new \Exception('Room not found');
        }
    }

    public function setRoomTemperature(string $id, float $temperature): bool
    {
        /*
            * This is a workaround to update the temperature of a room, as the CI4 automatically parses the jason with {} and this throws an error
            */
        try {
            $ch = curl_init();
            curl_setopt($ch, CURLOPT_URL, 'http://mainunit:8080/api/rooms/' . $id . '/temperature');
            curl_setopt($ch, CURLOPT_HTTPHEADER, array('Content-Type: application/json', 'Content-Length: ' . strlen($temperature)));
            curl_setopt($ch, CURLOPT_CUSTOMREQUEST, 'PUT');
            curl_setopt($ch, CURLOPT_POSTFIELDS, $temperature);
            curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
            curl_exec($ch);
            curl_close($ch);
        } catch (\Exception $e) {
            return false;
        }
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

    public function updateRoom(string $id, string $name, float $temperature): bool
    {
        try {
            $client = \Config\Services::curlrequest();
            $response = $client->request('PUT', 'http://mainunit:8080/api/rooms', [
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
            $response = $client->request('GET', 'http://mainunit:8080/api/RoomTemperature?start=2022-01-01&end=' . $currentDate);
        } catch (\Exception $e) {
            return [];
        }
    }
}