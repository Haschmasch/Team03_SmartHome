<?= $this->extend('layout/auth-layout'); ?>
<?php $this->section('content'); ?>

    <div class="login-box bg-white box-shadow border-radius-10">
        <div class="login-title">
            <h2 class="text-center text-primary">Anmelden</h2>
        </div>
        <?php $validation = \Config\Services::validation(); ?>
        <form action="<?= base_url(route_to('login.handler')) ?>" method="POST">
            <?= csrf_field() ?>

            <!-- Flash Messages -->

            <?php if (!empty(session()->getFlashdata('success'))): ?>
                <div class="alert alert-success">
                    <?= session()->getFlashdata('success') ?>
                    <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                        <span>&times;</span>
                </div>
            <?php elseif (!empty(session()->getFlashdata('fail'))): ?>
                <div class="alert alert-danger">
                    <?= session()->getFlashdata('fail') ?>
                    <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                        <span>&times;</span>
                </div>
            <?php endif; ?>

            <!-- Username -->

            <?php if ($validation->getError('login_id')): ?>
                <div class="d-block text-danger" style="margin-top:-25px;margin-bottom: 15px">
                    <?= $validation->getError('login_id') ?>
                </div>
            <?php endif; ?>
            <div class="input-group custom">
                <input type="text" class="form-control form-control-lg" placeholder="Username" name="login_id"
                       value="<?= set_value('login_id') ?>" required/>
                <div class="input-group-append custom">
                <span class="input-group-text">
                    <i class="icon-copy dw dw-user1"></i>
                </span>
                </div>
            </div>

            <!-- Password -->

            <?php if ($validation->getError('password')): ?>
                <div class="d-block text-danger" style="margin-top:-25px;margin-bottom: 15px">
                    <?= $validation->getError('password') ?>
                </div>
            <?php endif; ?>
            <div class="input-group custom">
                <input type="password" class="form-control form-control-lg" placeholder="**********" name="password"
                       value="<?= set_value('password') ?>" required/>
                <div class="input-group-append custom">
                <span class="input-group-text">
                    <i class="dw dw-padlock1"></i>
                </span>
                </div>
            </div>

            <!-- Actions -->

            <div class="row">
                <div class="col-sm-12">
                    <div class="input-group mb-0">

                        <input class="btn btn-primary btn-lg btn-block" type="submit" value="Anmleden">
                    </div>
                    <div class="font-16 weight-600 pt-10 pb-10 text-center" data-color="#707373">
                        oder
                    </div>
                    <div class="input-group mb-0">
                        <a class="btn btn-outline-primary btn-lg btn-block" href="<?=base_url(route_to('register.form'))?>">Account anlegen</a>
                    </div>
                </div>
            </div>
        </form>
    </div>

<?= $this->endSection(); ?>