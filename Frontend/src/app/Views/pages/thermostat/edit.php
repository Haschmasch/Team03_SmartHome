<?php
/**
 * @var \App\Entities\Thermostat $thermostat
 * @var \App\Entities\Room[] $rooms
 */
?>
<?= $this->extend('layout/pages-layout') ?>
<?= $this->section('content') ?>


<div class="title pb-20">
    <h2 class="h3 mb-0">Thermostat - <?= $thermostat->getID() ?> bearbeiten</h2>
</div>

<div class="pd-20 card-box mb-30">
    <?php $validation = \Config\Services::validation(); ?>
    <form action="<?=base_url('thermostat/edit/') . $thermostat->getID()?>" method="POST">
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
            <div class="col-md-10 col-sm-12">
                <div class="form-group">
                    <label>Zugewiesenes Zimmer</label>
                    <select class="custom-select" name="room" id="room">
                        <option value="0"<?= is_null($thermostat->getID()) ? ' selected' : '' ?>>Nicht zugewisen</option>
                        <?php foreach ($rooms as $room): ?>
                        <option value="<?= $room->getID() ?>" <?= $thermostat->getRoomID() === $room->getID() ? 'selected' : '' ?>><?= $room->getName() ?></option>
                        <?php endforeach; ?>
                    </select>
                </div>
                <?php if ($validation->getError('room')): ?>
                    <div class="d-block text-danger" style="margin-top: 15px;margin-bottom: 15px">
                        <?= $validation->getError('room') ?>
                    </div>
                <?php endif; ?>
            </div>
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