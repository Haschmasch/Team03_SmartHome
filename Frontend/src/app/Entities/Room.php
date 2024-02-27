<?php

namespace app\Entities;


/**
 * @property int $ID
 * @property string $name
 * @property float $temperature
 * @property int[] $thermostats
 */
class Room
{
    private int $ID;
    private string $name;
    private float $temperature;
    private array $thermostats;

    public function __construct(int $ID, string $name, float $temperature, array $thermostats)
    {
        $this->ID = $ID;
        $this->name = $name;
        $this->temperature = $temperature;
        $this->thermostats = $thermostats;
    }

    public function getID(): int
    {
        return $this->ID;
    }

    public function getName(): string
    {
        return $this->name;
    }

    public function getTemperature(): float
    {
        return $this->temperature;
    }

    public function setTemperature(float $temperature): void
    {
        $this->temperature = $temperature;
    }

    public function getThermostats(): array
    {
        return $this->thermostats;
    }

    public function setThermostats(array $thermostats): void
    {
        $this->thermostats = $thermostats;
    }
}