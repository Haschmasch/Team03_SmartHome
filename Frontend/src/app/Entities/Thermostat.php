<?php

namespace app\Entities;


/**
 * @property int $ID
 * @property float $temperature
 * @property int $roomID
 */
class Thermostat
{
    private int $ID;
    private float $temperature;
    private ?int $roomID;

    /**
     * @param int $ID
     * @param float $temperature
     * @param ?int $roomID
     */
    public function __construct(int $ID, float $temperature, ?int $roomID)
    {
        $this->ID = $ID;
        $this->temperature = $temperature;
        $this->roomID = $roomID;
    }

    public function getID(): int
    {
        return $this->ID;
    }

    public function getTemperature(): float
    {
        return $this->temperature;
    }

    public function getRoomID(): ?int
    {
        return $this->roomID;
    }
}