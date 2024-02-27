<?php

use CodeIgniter\Router\RouteCollection;

/**
 * @var RouteCollection $routes
 */

$routes->group('', static function ($routes) {
    $routes->group('', ['filter' => 'cifilter:auth'], static function ($routes) {
        $routes->get('/', 'Home::index');
        $routes->get('home', 'Home::index', ['as' => 'home']);
        $routes->get('thermostats', 'ThermostatController::index', ['as' => 'thermostats']);
        $routes->group('thermostat', static function ($routes) {
            $routes->get('', 'ThermostatController::view', ['as' => 'thermostat']);
            $routes->get('create', 'ThermostatController::create', ['as' => 'thermostat.create']);
            $routes->get('edit/(:any)', 'ThermostatController::edit/$1', ['as' => 'thermostat.edit']);
            $routes->post('edit/(:any)', 'ThermostatController::edit/$1', ['as' => 'thermostat.edit']);
        });
        $routes->group('room', static function ($routes) {
            $routes->get('', 'ThermostatController::view', ['as' => 'room']);
            $routes->get('create', 'RoomController::create', ['as' => 'room.create']);
            $routes->post('create', 'RoomController::create', ['as' => 'room.create']);
            $routes->get('edit/(:any)', 'RoomController::edit/$1', ['as' => 'room.edit']);
            $routes->post('edit/(:any)', 'RoomController::edit/$1', ['as' => 'room.edit']);
        });
        $routes->get('logout', 'AuthController::logoutHandler', ['as' => 'logout']);
    });
    $routes->group('', ['filter' => 'cifilter:guest'], static function ($routes) {
        $routes->get('login', 'AuthController::loginForm', ['as' => 'login.form']);
        $routes->post('login', 'AuthController::loginHandler', ['as' => 'login.handler']);
        $routes->get('register', 'AuthController::registerForm', ['as' => 'register.form']);
        $routes->post('register', 'AuthController::registerHandler', ['as' => 'register.handler']);
    });
});
