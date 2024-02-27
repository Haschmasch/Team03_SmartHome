<?php

namespace app\Models;

use App\Entities\User;
class UserModel
{
    public function __construct()
    {
    }

    public function getUser(string $username): User
    {
        #todo: implement query to get user
        return new User(1, 'Max Mustermann', 'max@mustermann.de');
    }

    public function checkCredentials(string $username, string $password): bool
    {
        #todo: implement query to check credentials
        return true;
    }

    public function registerUser(string $username, string $email, string $password): bool
    {
        #todo: implement query to register user
        return true;
    }
}