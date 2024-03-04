<!DOCTYPE html>
<html lang="en">
<head>
    <!-- Basic Page Info -->
    <meta charset="utf-8"/>
    <title>Heathub | <?= $pageTitle ?? 'Heathub'; ?></title>

    <!-- Site favicon -->
    <link rel="apple-touch-icon" sizes="180x180" href="/vendors/images/HeatHubicon.png"/>
    <link rel="icon" type="image/png" href="/vendors/images/HeatHubIcon.png"/>

    <!-- Mobile Specific Metas -->
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1"/>

    <!-- Google Font -->
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@300;400;500;600;700;800&display=swap"
          rel="stylesheet"/>
    <!-- CSS -->
    <link rel="stylesheet" type="text/css" href="/vendors/styles/core.css"/>
    <link rel="stylesheet" type="text/css" href="/vendors/styles/icon-font.min.css"/>
    <link rel="stylesheet" type="text/css" href="/vendors/styles/style.css"/>
    <?= $this->renderSection('stylesheets') ?>
</head>
<body class="login-page">
<div class="login-header box-shadow">
    <div class="container-fluid d-flex justify-content-between align-items-center">
        <div class="brand-logo">
            <a href="login.html">
                <img src="/vendors/images/HeatHubLogo.png" alt="Heathub Logo"/>
            </a>
        </div>
    </div>
</div>
<div class="login-wrap d-flex align-items-center flex-wrap justify-content-center">
    <div class="container">
        <?= $this->renderSection('content') ?>
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
