<?php

namespace app\Entities;


/**
 * @property int $ID
 * @property float $temperature
 * @property int $roomID
 */
class Thermostat
{
    private string $ID;
    private float $temperature;
    private ?string $roomID;

    /**
     * @param string $ID
     * @param float $temperature
     * @param ?string $roomID
     */
    public function __construct(string $ID, float $temperature, ?string $roomID)
    {
        $this->ID = $ID;
        $this->temperature = $temperature;
        $this->roomID = $roomID;
    }

    public function getID(): string
    {
        return $this->ID;
    }

    public function getTemperature(): float
    {
        return $this->temperature;
    }

    public function getRoomID(): ?string
    {
        return $this->roomID;
    }
}