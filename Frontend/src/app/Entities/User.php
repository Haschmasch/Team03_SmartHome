<?php

namespace App\Entities;


/**
 * @property string $token
 * @property string $email
 */
class User
{
    private string $email;
    private string $token;

    public function __construct(string $token, string $email)
    {
        $this->token = $token;
        $this->email = $email;
    }

    public function getEmail(): string
    {
        return $this->email;
    }

    public function getToken(): string
    {
        return $this->token;
    }
}