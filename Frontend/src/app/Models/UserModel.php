<?php

namespace app\Models;

use App\Entities\User;
class UserModel
{
    public function __construct()
    {
    }

    public function checkCredentials(string $username, string $password): false|string
    {
        try {
            $client = \Config\Services::curlrequest();

            $response = $client->request('POST', 'http://mainunit:8080/api/auth/login', [
                'json' => [
                    'username' => $username,
                    'password' => $password
                ]
            ]);
            $user_response = json_decode($response->getBody());
            return $user_response->token;
        } catch (\Exception $e) {
            return false;
        }
    }

    public function registerUser(string $email, string $password): bool
    {
        try {
            $client = \Config\Services::curlrequest();

            $response = $client->request('POST', 'http://mainunit:8080/api/auth/register', [
                'json' => [
                    'username' => $email,
                    'password' => $password
                ]
            ]);
            return true;
        } catch (\Exception $e) {
            return false;
        }
    }
}