<!DOCTYPE html>
<html lang="en">
<head>
    <!-- Basic Page Info -->
    <meta charset="utf-8"/>
    <title>HeatHub<?= !is_null($pageTitle) ? ' | ' . $pageTitle : ''; ?></title>

    <!-- Site favicon -->
    <link rel="apple-touch-icon" sizes="180x180" href="/vendors/images/HeatHubIcon.png"/>
    <link rel="icon" type="image/png" href="/vendors/images/HeatHubIcon.png"/>

    <!-- Mobile Specific Metas -->
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1"/>

    <!-- Google Font -->
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@300;400;500;600;700;800&display=swap" rel="stylesheet"/>
    <!-- CSS -->
    <link rel="stylesheet" type="text/css" href="/vendors/styles/core.css"/>
    <link rel="stylesheet" type="text/css" href="/vendors/styles/icon-font.min.css"/>
    <link rel="stylesheet" type="text/css" href="/vendors/styles/style.css"/>
    <?= $this->renderSection('stylesheets') ?>
</head>
<body>

<?php include('inc/header.php') ?>
<?php include('inc/right-sidebar.php') ?>
<?php include('inc/left-sidebar.php') ?>

<div class=" mobile-menu-overlay"></div>

<div class="main-container">
    <div class="pd-ltr-20 xs-pd-20-10">
        <div class="min-height-200px">

            <div>
                <?= $this->renderSection('content') ?>
            </div>
        </div>
        <?php include('inc/footer.php') ?>
    </div>
</div>
<!-- js -->
<script src="/vendors/scripts/core.js"></script>
<script src="/vendors/scripts/script.min.js"></script>
<script src="/vendors/scripts/process.js"></script>
<script src="/vendors/scripts/layout-settings.js"></script>
<?= $this->renderSection('scripts') ?>
</body>
</html>