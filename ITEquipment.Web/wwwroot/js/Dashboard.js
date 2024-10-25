/*https://www.chartjs.org/docs/latest/charts/doughnut.html#pie*/
jQuery(document).ready(function () {
    //getStatusCount();
    loadChart1();
    loadChart2();
});

//function getStatusCount() {

//    $.ajax({
//        url: '/Home/GetStatusCounts', // The URL of your API endpoint or controller action
//        type: 'GET', // or 'POST' depending on your requirement
//        dataType: 'json',
//        success: function (response) {
//            console.log(response); // handle success
//        },
//        error: function (xhr, status, error) {
//            console.error('Error: ' + error); // handle error
//        }
//    });
//}

//function loadChart1() {
//    const ctx = document.getElementById('chartAssestDetail');

//    new Chart(ctx, {
//        type: 'bar',
//        data: {
//            labels: ['Red', 'Blue', 'Yellow', 'Green', 'Purple', 'Orange'],
//            datasets: [{
//                label: '# of Votes',
//                data: [12, 19, 3, 5, 2, 3],
//                borderWidth: 1
//            }]
//        },
//        options: {
//            scales: {
//                y: {
//                    beginAtZero: true
//                }
//            }
//        }
//    });
//}

//function loadChart2() {
//    const ctx = document.getElementById('chartAssestUsage');

//    const data = {
//        labels: [
//            'Red',
//            'Blue',
//            'Yellow'
//        ],
//        datasets: [{
//            label: 'My First Dataset',
//            data: [300, 50, 100],
//            backgroundColor: [
//                'rgb(255, 99, 132)',
//                'rgb(54, 162, 235)',
//                'rgb(255, 205, 86)'
//            ],
//            hoverOffset: 4
//        }]
//    };
//    const config = {
//        type: 'pie',
//        data: data,
//    };

//    new Chart(ctx, config);
//}


function loadChart1() {
    const ctx = document.getElementById('chartAssestDetail');

    // Make AJAX call to fetch the data dynamically
    $.ajax({
        url: '/Home/GetTopUsedEquipmentCountList', // Your API endpoint
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            // Extract labels (equipment names) and data (booking counts) from the response
            const labels = response.map(item => item.name);
            const dataValues = response.map(item => item.bookingCount);

            // Define an array of colors for the bars
            const colors = [
                'rgba(75, 192, 192, 0.5)',
                'rgba(255, 99, 132, 0.5)',
                'rgba(255, 205, 86, 0.5)',
                'rgba(54, 162, 235, 0.5)',
                'rgba(153, 102, 255, 0.5)',
                'rgba(255, 159, 64, 0.5)',
                'rgba(201, 203, 207, 0.5)', // Additional colors if needed
                'rgba(255, 165, 0, 0.5)',  // Orange
                'rgba(0, 128, 0, 0.5)',    // Green
                'rgba(128, 0, 128, 0.5)',  // Purple
                'rgba(0, 0, 255, 0.5)',    // Blue
            ];

            // Create the chart with the dynamic data
            new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: labels, // Equipment names
                    datasets: [{
                        label: 'Booking Count', // Label for the dataset
                        data: dataValues, // Booking counts
                        backgroundColor: colors.slice(0, labels.length), // Use different colors for each bar
                        borderColor: 'rgba(0, 0, 0, 1)', // Border color for all bars
                        borderWidth: 1
                    }]
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    plugins: {
                        title: {
                            display: true,
                            text: 'Top Used Equipment Booking Counts', // Chart title
                            font: {
                                size: 18 // Title font size
                            }
                        },
                        legend: {
                            display: true, // Show legend
                            position: 'top' // Position legend at the top
                        }
                    },
                    scales: {
                        y: {
                            beginAtZero: true // Start Y-axis at zero
                        }
                    }
                }
            });
        },
        error: function (xhr, status, error) {
            console.error('Error: ' + error); // Handle error
        }
    });
}





function loadChart2() {
    const ctx = document.getElementById('chartAssestUsage');

    // Make AJAX call to fetch the data dynamically
    $.ajax({
        url: '/Home/GetStatusCounts', // The URL of your API endpoint or controller action
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            // Extract labels (status types) and data (record counts) from the response
            const labels = response.map(item => item.statusType.trim()); // Trim to remove extra spaces if needed
            const dataValues = response.map(item => item.recordCount);

            // Calculate the total equipment count
            const totalCount = dataValues.reduce((total, count) => total + count, 0);

            // Define the chart data and configuration
            const data = {
                labels: labels,
                datasets: [{
                    label: 'Equipment Status Counts',
                    data: dataValues,
                    backgroundColor: [
                        'rgb(255, 99, 132)',    // Color for first item
                        'rgb(54, 162, 235)',    // Color for second item
                        'rgb(255, 205, 86)',    // Color for third item
                        'rgb(75, 192, 192)',    // Color for fourth item
                        'rgb(153, 102, 255)',   // Color for fifth item
                        'rgb(255, 159, 64)'     // Color for sixth item
                    ],
                    hoverOffset: 4
                }]
            };

            // Chart configuration with title and dataLabels plugin
            const config = {
                type: 'pie', // You can change to 'bar' or other chart types if desired
                data: data,
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    plugins: {
                        title: {
                            display: true, // Enable the title
                            text: `Equipment Status Overview (Total: ${totalCount})`, // Set the title with total count
                            font: {
                                size: 18, // Font size of the title
                                weight: 'bold' // Font weight (bold, normal, etc.)
                            },
                            padding: {
                                top: 10,
                                bottom: 30
                            },
                            color: '#333', // Title color
                        },
                        datalabels: {
                            display: true,
                            color: 'white', // Color of the labels on chart blocks
                            font: {
                                weight: 'bold'
                            },
                            formatter: function (value, context) {
                                // Show the status and the corresponding count in each pie block
                                const label = context.chart.data.labels[context.dataIndex];
                                return `${label}: ${value}`;
                            }
                        }
                    }
                }
            };

            // Initialize the chart with the dynamic data and configuration
            new Chart(ctx, config);
        },
        error: function (xhr, status, error) {
            console.error('Error: ' + error); // handle error
        }
    });
}

