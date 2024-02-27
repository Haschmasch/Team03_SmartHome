<?= $this->extend('layout/pages-layout') ?>
<?= $this->section('content') ?>


    <div class="title pb-20">
        <h2 class="h3 mb-0">Raum - Hinzufügen</h2>
    </div>

    <div class="pd-20 card-box mb-30">
        <?php $validation = \Config\Services::validation(); ?>
        <form action="<?= base_url('room/create/') ?>" method="POST">
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
                    <label class="col-sm-12 col-md-2 col-form-label" for="name">Raum Name</label>
                    <div class="col-sm-12 col-md-10">
                        <input class="form-control" type="text" placeholder="Raum Name" id="name" name="name">
                    </div>
                    <?php if ($validation->getError('name')): ?>
                        <div class="d-block text-danger" style="margin-top: 15px;margin-bottom: 15px">
                            <?= $validation->getError('name') ?>
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