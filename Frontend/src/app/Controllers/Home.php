<?php

namespace App\Controllers;

use App\Models\RoomModel;

class Home extends BaseController
{
    public function index(): string
    {
        return view('pages/home', [
            'pageTitle' => 'Dashboard',
            'rooms' => (new RoomModel())->getRooms(),
            'user' => [
                'name' => 'Max Mustermann',
                'email' => 'max@mustermann.de',
            ]
        ]);
    }
}
