<?php

namespace App\Models;

use App\Entities\Thermostat;

class ThermostatModel
{

    public function __construct()
    {
    }

    public function getThermostats(): array
    {
        return [
            new Thermostat(1, 16, 1),
            new Thermostat(2, 16, 1),
            new Thermostat(3, 16, 1),
            new Thermostat(4, 16, null),
        ];
    }

    public function setRoom(int $thermostatId, int $roomId): bool
    {
        return true;
    }

    public function getThermostat(string $id): Thermostat
    {
        switch ($id) {
            case 2:
                return new Thermostat(2, 16, 1);
            case 3:
                return new Thermostat(3, 16, 1);
            case 4:
                return new Thermostat(4, 16, null);
            default:
                return new Thermostat(1, 16, 1);
        }
    }
}