<?php

namespace App\Filters;

use App\Entities\User;

class CIAuth
{
    public static function setCIAuth($result): void
    {
        $session = session();
        $session->set('userdata', $result);
        $session->set('logged_in', true);
    }

    public static function id()
    {
        $session = session();
        if ($session->has('logged_in')) {
            if ($session->has('userdata')) {
                return $session->get('userdata')['id'];
            }
        }
        return null;
    }

    public static function isLoggedIn(): bool
    {
        $session = session();
        return $session->has('logged_in');
    }

    public static function forget(): void
    {
        $session = session();
        $session->remove('userdata');
        $session->remove('logged_in');
    }

    public static function user()
    {
        $session = session();
        if ($session->has('logged_in')) {
            if ($session->has('userdata')) {
                return $session->get('userdata');
            }
        }
        return null;
    }
}