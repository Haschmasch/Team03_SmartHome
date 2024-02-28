<?php

namespace App\Entities;


/**
 * @property string $username
 * @property string $email
 */
class User
{
    private int $id;
    private string $username;
    private string $email;

    public function __construct(string $username, string $email)
    {
        $this->username = $username;
        $this->email = $email;
    }

    public function getUsername(): string
    {
        return $this->username;
    }
}