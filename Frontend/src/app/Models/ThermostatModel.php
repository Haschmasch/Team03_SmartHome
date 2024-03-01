<?php

namespace App\Models;

use app\Entities\Room;
use App\Entities\Thermostat;
use App\Filters\CIAuth;

class ThermostatModel
{

    private CIAuth $CIAuth;

    public function __construct()
    {
        $this->CIAuth = new CIAuth();
    }

    public function getThermostats(): array
    {
        try {
            $client = \Config\Services::curlrequest();

            $response = $client->request('GET', 'http://mainunit:8080/api/thermostats?skip=0&limit=100', [
                'headers' => [
                    'authorization' => 'Bearer ' . $this->CIAuth->user()->getToken(),
                ],
            ]);
            $thermostats = [];
            foreach (json_decode($response->getBody()) as $thermostat) {
                $thermostats[] = new Thermostat(
                    $thermostat->id,
                    (float)$thermostat->temperature,
                    $thermostat->roomId);
            }
            return $thermostats;
        } catch (\Exception $e) {
            return [];
        }
    }

    public function setRoom(string $thermostatId, string $roomId): bool
    {
        /*
            * This is a workaround to update the temperature of a room, as the CI4 automatically parses the jason with {} and this throws an error
            */
        try {
            $authorization = 'Authorization: Bearer ' . $this->CIAuth->user()->getToken();
            $ch = curl_init();
            curl_setopt($ch, CURLOPT_URL, 'http://mainunit:8080/api/rooms/' . $roomId . '/thermostats');
            curl_setopt($ch, CURLOPT_HTTPHEADER, array('Content-Type: application/json', $authorization, 'Content-Length: ' . strlen($thermostatId)));
            curl_setopt($ch, CURLOPT_CUSTOMREQUEST, 'PUT');
            curl_setopt($ch, CURLOPT_POSTFIELDS, $thermostatId);
            curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
            curl_exec($ch);
            curl_close($ch);
        } catch (\Exception $e) {
            return false;
        }
        return true;
    }

    public function getThermostat(string $id): Thermostat
    {
        try {
            $client = \Config\Services::curlrequest();

            $response = $client->request('GET', 'http://mainunit:8080/api/thermostats/' . $id, [
                'headers' => [
                    'authorization' => 'Bearer ' . $this->CIAuth->user()->getToken(),
                ],
            ]);

            $thermostatResponse = json_decode($response->getBody());
            return new Thermostat(
                $thermostatResponse->id,
                (float)$thermostatResponse->temperature,
                $thermostatResponse->roomId);

        } catch (\Exception $e) {
            throw new \Exception('Room not found');
        }
    }
}