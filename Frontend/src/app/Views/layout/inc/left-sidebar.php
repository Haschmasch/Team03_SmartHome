<div class="left-side-bar">
    <div class="brand-logo">
        <a href="index.html">
            <img src="/vendors/images/HeatHubLogo.png" alt=""/>
        </a>
        <div class="close-sidebar" data-toggle="left-sidebar-close">
            <i class="ion-close-round"></i>
        </div>
    </div>
    <div class="menu-block customscroll">
        <div class="sidebar-menu">
            <ul id="accordion-menu">
                <li>
                    <a href="<?= base_url(route_to('home')) ?>" class="dropdown-toggle no-arrow">
                        <span class="micon bi bi-house"></span>
                        <span class="mtext">Dashboard</span>
                    </a>
                </li>
                <li>
                    <a href="<?= base_url(route_to('rooms')) ?>" class="dropdown-toggle no-arrow">
                        <span class="micon bi bi-door-open"></span>
                        <span class="mtext">Räume</span>
                    </a>
                </li>
                <li>
                    <a href="<?= base_url(route_to('thermostats')) ?>" class="dropdown-toggle no-arrow">
                        <span class="micon bi bi-speedometer"></span>
                        <span class="mtext">Thermostate</span>
                    </a>
                </li>
            </ul>
        </div>
    </div>
</div>