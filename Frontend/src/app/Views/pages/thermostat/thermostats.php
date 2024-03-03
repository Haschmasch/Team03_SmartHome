<?php
/**
 * @var \App\Entities\Thermostat[] $thermostats
 * @var \App\Entities\Room[] $rooms
 */
?>

<?= $this->extend('layout/pages-layout') ?>
<?= $this->section('content') ?>

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

<div class="title pb-20">
    <h2 class="h3 mb-0">Meine Thermostate</h2>
</div>
<div class="row pb-10">

    <?php
    $counter = 1;
    foreach ($thermostats as $thermostat): ?>
        <div class="col-sm-12 col-md-4 mb-30">
            <div class="card card-box">
                <div class="card-header"><?= !is_null($thermostat->getRoomID()) ? $rooms[$thermostat->getRoomID()]->getName() : "Nicht zugewisen" ?></div>
                <div class="card-body">
                    <h5 class="card-title">Thermostat - <?= $counter?></h5>
                    <p class="card-text"><?= $thermostat->getTemperature() ?> &deg;C</p>
                    <a href="<?= base_url(route_to('thermostat.edit', $thermostat->getID())) ?>"
                       class="btn btn-primary">Bearbeiten</a>
                </div>
            </div>
        </div>
    <?php
    $counter++;
    endforeach; ?>
</div>
<?= $this->endSection() ?>
