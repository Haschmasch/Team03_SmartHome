<?php

namespace App\Controllers;

use App\Filters\CIAuth;
use App\Models\RoomModel;


class Home extends BaseController
{
    public function __construct()
    {
        $this->CIAuth = new CIAuth();
    }
    public function index(): string
    {
        return view('pages/home', [
            'pageTitle' => 'Dashboard',
            'rooms' => (new RoomModel())->getRooms(),
            'user' => $this->CIAuth->user(),
        ]);
    }
}
