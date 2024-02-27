<?php

namespace App\Filters;

use CodeIgniter\Filters\FilterInterface;
use CodeIgniter\HTTP\RequestInterface;
use CodeIgniter\HTTP\ResponseInterface;
use App\Filters\CIAuth;


class CIFilter implements FilterInterface
{
    public function before(RequestInterface $request, $arguments = null)
    {
        $CIAuth = new CIAuth();
        if ($arguments[0] == 'guest') {
            if ($CIAuth::isLoggedIn()) {
                return redirect()->route('home');
            }
        }

        if ($arguments[0] == 'auth') {
            if (!$CIAuth::isLoggedIn()) {
                return redirect()->route('login.form')->with('fail', 'Anmeldung erforderlich!');
            }
        }
    }

    public function after(RequestInterface $request, ResponseInterface $response, $arguments = null)
    {
        // Do something here
    }
}
