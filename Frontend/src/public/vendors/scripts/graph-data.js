var options = {
	series : graphData.series,
	chart: {
		height: 300,
		type: 'line',
		zoom: {
			enabled: false,
		},
		dropShadow: {
			enabled: true,
			color: '#000',
			top: 18,
			left: 7,
			blur: 16,
			opacity: 0.2
		},
		toolbar: {
			show: false
		}
	},
	colors: ['#F0746C', '#D16587', '#A16193', '#6E5D8D', '#445577', '#2F4858'],
	dataLabels: {
		enabled: false,
	},
	stroke: {
		width: [3,3],
		curve: 'smooth'
	},
	grid: {
		show: false,
	},
	markers: {
		colors: ['#F0746C', '#D16587', '#A16193', '#6E5D8D', '#445577', '#2F4858'],
		size: 5,
		strokeColors: '#ffffff',
		strokeWidth: 2,
		hover: {
			sizeOffset: 2
		}
	},
	xaxis: {
		categories: graphData.categories,
		labels:{
			style:{
				colors: '#8c9094'
			}
		}
	},
	yaxis: {
		min: 0,
		max: 35,
		labels:{
			style:{
				colors: '#8c9094'
			}
		}
	},
	legend: {
		position: 'top',
		horizontalAlign: 'right',
		floating: true,
		offsetY: 0,
		labels: {
			useSeriesColors: true
		},
		markers: {
			width: 10,
			height: 10,
		}
	}
};

var chart = new ApexCharts(document.querySelector("#activities-chart"), options);
chart.render();

// datatable init
$('document').ready(function(){
	$('.data-table').DataTable({
		scrollCollapse: false,
		autoWidth: false,
		responsive: true,
		searching: false,
		bLengthChange: false,
		bPaginate: true,
		bInfo: false,
		columnDefs: [{
			targets: "datatable-nosort",
			orderable: false,
		}],
		"lengthMenu": [[5, 25, 50, -1], [5, 25, 50, "All"]],
		"language": {
			"info": "_START_-_END_ of _TOTAL_ entries",
			searchPlaceholder: "Search",
			paginate: {
				next: '<i class="ion-chevron-right"></i>',
				previous: '<i class="ion-chevron-left"></i>'
			}
		},
	});
});