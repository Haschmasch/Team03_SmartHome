<?php

namespace app\Entities;


/**
 * @property int $ID
 * @property string $username
 * @property string $email
 */
class User
{
    private int $id;
    private string $username;
    private string $email;

    public function __construct(int $id, string $username, string $email)
    {
        $this->id = $id;
        $this->username = $username;
        $this->email = $email;
    }

    public function getUsername(): string
    {
        return $this->username;
    }
}