<?php
/**
 * @var \App\Entities\Room $room
 */
?>
<?= $this->extend('layout/pages-layout') ?>
<?= $this->section('content') ?>


    <div class="title pb-20">
        <h2 class="h3 mb-0"><?= $room->getName() ?> bearbeiten</h2>
    </div>

    <div class="pd-20 card-box mb-30">
        <?php $validation = \Config\Services::validation(); ?>
        <form action="<?= base_url('room/edit/') . $room->getID() ?>" method="POST">
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

            <!-- Room -->

            <div class="row">
                <div class="col-md-6 col-sm-12">
                    <div class="form-group">
                        <label>Raum Name</label>
                        <input class="form-control" type="text" placeholder="Raum Name" id="name" name="name"
                               value="<?= set_value('name', $room->getName()) ?>">
                    </div>


                    <?php if ($validation->getError('name')): ?>
                        <div class="d-block text-danger" style="margin-top: 15px;margin-bottom: 15px">
                            <?= $validation->getError('name') ?>
                        </div>
                    <?php endif; ?>

                </div>
                <div class="col-md-6 col-sm-12">
                    <div class="form-group">
                        <label>Temperatur</label>
                        <input type="number" class="form-control" name="temperature" id="temperature"
                               value="<?= set_value('temperature', $room->getTemperature())?>">
                    </div>
                </div>
                <?php if ($validation->getError('temperature')): ?>
                    <div class="d-block text-danger" style="margin-top: 15px;margin-bottom: 15px">
                        <?= $validation->getError('temperature') ?>
                    </div>
                <?php endif; ?>

            </div>


            <div class="row">
                <div class="col-md-2 col-sm-12">
                    <div class="form-group">
                        <label>Bestätigen</label>
                        <input type="submit" class="btn btn-success" value="Bestätigen">
                    </div>
                </div>
            </div>


        </form>
    </div>
<?= $this->endSection() ?>