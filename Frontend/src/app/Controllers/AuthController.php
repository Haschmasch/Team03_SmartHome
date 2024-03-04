<?php

namespace App\Controllers;

use App\Entities\User;
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
            $token = $userModel->checkCredentials(
                $this->request->getPost('login_id'),
                $this->request->getPost('password')
            );



            if (!$token) {
                return redirect()->route('login.form')->with('fail', 'Invalid email or password')->withInput();
            } else {
                $user = new User($token, $this->request->getPost('login_id'));
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
                'rules' => 'required|min_length[8]|matches[password_confirm]',
                'errors' => [
                    'required' => 'Bitte gib dein Passwort ein',
                    'min_length' => 'Passwort muss mindestens 8 Zeichen lang sein',
                    'matches' => 'Passwörter stimmen nicht überein'
                ]
            ],
            'password_confirm' => [
                'rules' => 'required|matches[password]|min_length[8]',
                'errors' => [
                    'required' => 'Bitte gib dein Passwort ein',
                    'min_length' => 'Passwort muss mindestens 8 Zeichen lang sein',
                    'matches' => 'Passwörter stimmen nicht überein'
                ]
            ]
        ]);

        if (!$isValid) {
            return view('pages/auth/register', [
                'pageTitle' => 'Registrieren',
                'validation' => $this->validator
            ]);
        } else {
            $token = $userModel->registerUser(
                $this->request->getPost('login_id'),
                $this->request->getPost('password')
            );



            if (!$token) {
                return redirect()->route('register.form')->with('fail', 'Registrieren fehlgeschlagen')->withInput();
            } else {
                $user = new User($token, $this->request->getPost('login_id'));
                CIAuth::setCIAuth($user);
                CIAuth::forget();
                return redirect()->route('login.form')->with('success', 'Account erstellt')->withInput();
            }
        }
    }
    public function logoutHandler(): RedirectResponse
    {
        CIAuth::forget();
        return redirect()->route('login.form')->with('fail', 'Loggout erfolgreich!');
    }
}