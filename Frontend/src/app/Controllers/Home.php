<?php

namespace App\Controllers;

use App\Filters\CIAuth;
use App\Models\RoomModel;


class Home extends BaseController
{
    private RoomModel $roomModell;
    private CIAuth $CIAuth;

    public function __construct()
    {
        $this->CIAuth = new CIAuth();
        $this->roomModell = new RoomModel();
    }
    public function index(): string
    {
        $foo = $this->roomModell->getTemperatureData();

        return view('pages/home', [
            'pageTitle' => 'Dashboard',
            'rooms' => (new RoomModel())->getRooms(),
            'user' => $this->CIAuth->user(),
        ]);
    }
}
