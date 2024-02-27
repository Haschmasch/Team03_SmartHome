<?= $this->extend('layout/pages-layout') ?>
<?= $this->section('content') ?>
<?php
function selectColor(float $temperature): string
{
    $colorLookup = [
        'colder' => '00aff0',
        'cold' => '007bb5',
        'normal' => '00b489',
        'warm' => 'f46f30',
        'warmer' => 'bd081c',
    ];

    switch ($temperature) {
        case $temperature < 15:
            return $colorLookup['colder'];
        case $temperature < 20:
            return $colorLookup['cold'];
        case $temperature < 25:
            return $colorLookup['normal'];
        case $temperature < 30:
            return $colorLookup['warm'];
        default:
            return $colorLookup['warmer'];
    }
}
?>

<div class="title pb-20">
    <h2 class="h3 mb-0">Meine Räume</h2>
</div>
<div class="row pb-10">
    <?php foreach ($rooms as $room): ?>
        <div class="col-xl-3 col-lg-3 col-md-6 mb-20">
            <div class="card-box height-100-p widget-style3">
                <div class="d-flex flex-wrap">
                    <div class="widget-data">
                        <div class="weight-700 font-24 text-dark"><?= $room->getTemperature() ?> &deg;C</div>
                        <div class="font-14 text-secondary weight-500">
                            <?= $room->getName() ?>
                        </div>
                    </div>
                    <div class="widget-icon">
                        <div class="icon" data-color="#<?= selectColor($room->getTemperature()) ?>">
                            <i class="icon-copy dw dw-thermometer"></i>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    <?php endforeach; ?>
    <div class="col-xl-3 col-lg-3 col-md-6 mb-20">
        <div class="card-box height-100-p widget-style3">
            <a href="<?=base_url(route_to('room.create'))?>">
                <div class="d-flex flex-wrap">
                    <div class="widget-data">
                        <div class="weight-700 font-24 text-dark">Neu</div>
                        <div class="font-14 text-secondary weight-500">
                            Raum hinzufügen
                        </div>
                    </div>
                    <div class="widget-icon">
                        <div class="icon" data-color="#09cc06">
                            <i class="icon-copy dw dw-add"></i>
                        </div>
                    </div>
                </div>
            </a>
        </div>
    </div>
</div>
<?= $this->endSection() ?>
