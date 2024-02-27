<?php

namespace App\Controllers;

use App\Models\UserModel;
use CodeIgniter\HTTP\RedirectResponse;
use App\Filters\CIAuth;

class AuthController extends BaseController
{
    protected $helpers = ['form', 'url'];

    public function loginForm(): string
    {
        $data = [
            'pageTitle' => 'Login',
            'validation' => null
        ];

        return view('pages/auth/login', $data);
    }

    public function registerForm(): string
    {
        $data = [
            'pageTitle' => 'Registrieren',
            'validation' => null
        ];

        return view('pages/auth/register', $data);
    }

    public function loginHandler(): RedirectResponse|string
    {
        $userModel = new UserModel();
        $isValid = $this->validate([
            'login_id' => [
                'rules' => 'required|valid_email',
                'errors' => [
                    'required' => 'Please enter your email',
                    'valid_email' => 'Please enter a valid email'
                ]
            ],
            'password' => [
                'rules' => 'required',
                'errors' => [
                    'required' => 'Please enter your password'
                ]
            ]
        ]);

        if (!$isValid) {
            return view('pages/auth/login', [
                'pageTitle' => 'Login',
                'validation' => $this->validator
            ]);
        } else {
            $is_user = $userModel->checkCredentials(
                $this->request->getPost('login_id'),
                $this->request->getPost('password')
            );

            $user = $userModel->getUser($this->request->getPost('login_id'));

            if (!$is_user) {
                return redirect()->route('login.form')->with('fail', 'Invalid email or password')->withInput();
            } else {
                CIAuth::setCIAuth($user);
                return redirect()->route('home');
            }
        }
    }

    public function registerHandler(): RedirectResponse|string
    {
        $userModel = new UserModel();
        $isValid = $this->validate([
            'login_id' => [
                'rules' => 'required|valid_email',
                'errors' => [
                    'required' => 'Bitte gib deine E-Mail-Adresse ein',
                    'valid_email' => 'Valide E-Mail-Adresse erforderlich'
                ]
            ],
            'password' => [
                'rules' => 'required',
                'errors' => [
                    'required' => 'Bitte gib dein Passwort ein'
                ]
            ]
        ]);

        if (!$isValid) {
            return view('pages/auth/login', [
                'pageTitle' => 'Login',
                'validation' => $this->validator
            ]);
        } else {
            $is_user = $userModel->checkCredentials(
                $this->request->getPost('login_id'),
                $this->request->getPost('password')
            );

            $user = $userModel->getUser($this->request->getPost('login_id'));

            if (!$is_user) {
                return redirect()->route('login.form')->with('fail', 'Login fehlgeschlagen')->withInput();
            } else {
                CIAuth::setCIAuth($user);
                return redirect()->route('home');
            }
        }
    }
    public function logoutHandler(): RedirectResponse
    {
        CIAuth::forget();
        return redirect()->route('login.form')->with('fail', 'Loggout erfolgreich!');
    }
}