document.addEventListener("DOMContentLoaded", function () {
    //definições da tabela em Workers/CreateTeam 
    let createTeamTable = new DataTable('#createTeamTable', {
        scrollY: '400px',
        scrollCollapse: true,
        paging: false,
        searching: true,
        columnDefs: [
            {
                targets: -1, 
                searchable: false 
            }
        ],
        language: {
            "decimal": ",",
            "thousands": ".",
            "lengthMenu": "Mostrar _MENU_ registos por página",
            "zeroRecords": "Nenhum registo encontrado",
            "info": "A mostrar _START_ a _END_ de _TOTAL_ registos",
            "infoEmpty": "A mostrar 0 a 0 de 0 registos",
            "infoFiltered": "(filtrado de _MAX_ registos no total)",
            "search": "Procurar:",
            "paginate": {
                "first": "Primeiro",
                "last": "Último",
                "next": "Seguinte",
                "previous": "Anterior"
            },
            "emptyTable": "Sem dados disponíveis na tabela",
            "loadingRecords": "A carregar...",
            "processing": "A processar...",
            "aria": {
                "sortAscending": ": ativar para ordenar a coluna de forma ascendente",
                "sortDescending": ": ativar para ordenar a coluna de forma descendente"
            }
        }
    });



    //definições da tabela em Workers/TeamDetails 
    let teamDetailsTable = new DataTable('#teamDetailsTable', {
        scrollY: '400px',
        scrollCollapse: true,
        paging: false,
        searching: true,
        columnDefs: [
            {
                targets: -1,
                searchable: false
            }
        ],
        language: {
            "decimal": ",",
            "thousands": ".",
            "lengthMenu": "Mostrar _MENU_ registos por página",
            "zeroRecords": "Nenhum registo encontrado",
            "info": "A mostrar _START_ a _END_ de _TOTAL_ registos",
            "infoEmpty": "A mostrar 0 a 0 de 0 registos",
            "infoFiltered": "(filtrado de _MAX_ registos no total)",
            "search": "Procurar:",
            "paginate": {
                "first": "Primeiro",
                "last": "Último",
                "next": "Seguinte",
                "previous": "Anterior"
            },
            "emptyTable": "Sem dados disponíveis na tabela",
            "loadingRecords": "A carregar...",
            "processing": "A processar...",
            "aria": {
                "sortAscending": ": ativar para ordenar a coluna de forma ascendente",
                "sortDescending": ": ativar para ordenar a coluna de forma descendente"
            }
        }
    });

    //definições da tabela equipas em Shift/_TeamsTable 
    let createShiftTeamTableConfig = new DataTable('#createShiftTeamTable', {
        scrollY: '400px',
        scrollCollapse: true,
        paging: false,
        searching: true,
        columnDefs: [
            {
                targets: -1,
                searchable: false
            }
        ],
        language: {
            "decimal": ",",
            "thousands": ".",
            "lengthMenu": "Mostrar _MENU_ registos por página",
            "zeroRecords": "Nenhum registo encontrado",
            "info": "A mostrar _START_ a _END_ de _TOTAL_ registos",
            "infoEmpty": "A mostrar 0 a 0 de 0 registos",
            "infoFiltered": "(filtrado de _MAX_ registos no total)",
            "search": "Procurar:",
            "paginate": {
                "first": "Primeiro",
                "last": "Último",
                "next": "Seguinte",
                "previous": "Anterior"
            },
            "emptyTable": "Sem dados disponíveis na tabela",
            "loadingRecords": "A carregar...",
            "processing": "A processar...",
            "aria": {
                "sortAscending": ": ativar para ordenar a coluna de forma ascendente",
                "sortDescending": ": ativar para ordenar a coluna de forma descendente"
            }
        }
    });

    //definições da tabela trabalhadores em Shift/_WorkersTeam 
    let createShiftWorkerTableConfig = new DataTable('#createShiftWorkerTable', {
        scrollY: '400px',
        scrollCollapse: true,
        paging: false,
        searching: true,
        columnDefs: [
            {
                targets: -1,
                searchable: false
            }
        ],
        language: {
            "decimal": ",",
            "thousands": ".",
            "lengthMenu": "Mostrar _MENU_ registos por página",
            "zeroRecords": "Nenhum registo encontrado",
            "info": "A mostrar _START_ a _END_ de _TOTAL_ registos",
            "infoEmpty": "A mostrar 0 a 0 de 0 registos",
            "infoFiltered": "(filtrado de _MAX_ registos no total)",
            "search": "Procurar:",
            "paginate": {
                "first": "Primeiro",
                "last": "Último",
                "next": "Seguinte",
                "previous": "Anterior"
            },
            "emptyTable": "Sem dados disponíveis na tabela",
            "loadingRecords": "A carregar...",
            "processing": "A processar...",
            "aria": {
                "sortAscending": ": ativar para ordenar a coluna de forma ascendente",
                "sortDescending": ": ativar para ordenar a coluna de forma descendente"
            }
        }
    });







    // Event listener para checkbox tabela trabalhadores
    $('#createShiftWorkerTable').on('change', '.worker-checkbox', function () {
        updateTableTeam();
    });

    // Event listener para checkbox tabela equipas
    $('#createShiftTeamTable').on('change', '.team-checkbox', function () {
        updateTableWorker();
    });

    function updateTableTeam() {
        // Get trabalhadores selecionados 
        const selectedWorkers = [];
        $('#createShiftWorkerTable .worker-checkbox').each(function () {
            const workerRow = $(this).closest('tr'); // Get row
            const workerId = workerRow.find('.worker-id').val(); // Get ID
            const isSelected = $(this).is(':checked'); // Get checkbox 
            const workerName = workerRow.find('.worker-name').val(); // Get Name
            const workerEmail = workerRow.find('.worker-email').text().trim(); // Get Email 

            selectedWorkers.push({
                Name: workerName,
                Email: workerEmail,
                Id: workerId,
                isSelected: isSelected
            });
        });

        // Get equipas selecionadas
        const selectedTeams = [];
        $('#createShiftTeamTable .team-checkbox').each(function () {
            const teamRow = $(this).closest('tr');
            const teamId = teamRow.find('span.d-none').text(); // Get ID
            const teamName = teamRow.find('.team-name').text();
            const isSelected = $(this).is(':checked');

            selectedTeams.push({
                Name: teamName,
                Id: teamId,
                isSelected: isSelected
            });
        });

        // Ativar o AJAX
        updateTeams(selectedWorkers, selectedTeams);
    }

    function updateTableWorker() {
        // Get trabalhadores selecionados 
        const selectedWorkers = [];
        $('#createShiftWorkerTable .worker-checkbox').each(function () {
            const workerRow = $(this).closest('tr'); // Get row
            const workerId = workerRow.find('.worker-id').val(); // Get ID
            const isSelected = $(this).is(':checked'); // Get checkbox 
            const workerName = workerRow.find('.worker-name').val(); // Get Name
            const workerEmail = workerRow.find('.worker-email').text().trim(); // Get Email 

            selectedWorkers.push({
                Name: workerName,
                Email: workerEmail,
                Id: workerId,
                isSelected: isSelected
            });
        });

        // Get equipas selecionadas
        const selectedTeams = [];
        $('#createShiftTeamTable .team-checkbox').each(function () {
            const teamRow = $(this).closest('tr');
            const teamId = teamRow.find('span.d-none').text(); // Get ID
            const teamName = teamRow.find('.team-name').text();
            const isSelected = $(this).is(':checked');

            selectedTeams.push({
                Name: teamName,
                Id: teamId,
                isSelected: isSelected
            });
        });

        // Ativar o AJAX
        updateWorkers(selectedWorkers, selectedTeams);
    }

    function updateWorkers(selectedWorkers, selectedTeams) {
        $.ajax({
            url: '/Shift/UpdateWorkers', 
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({
                selectedWorkers: selectedWorkers,
                selectedTeams: selectedTeams
            }),
            success: function (response) {
                $('#workerTableContainer').html(response);

                restartWorkersTable()
            },
            error: function (xhr, status, error) {
                console.error('Error updating workers:', error);
            }

        });
    }

    function updateTeams(selectedWorkers, selectedTeams) {
        $.ajax({
            url: '/Shift/UpdateTeams', 
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({
                selectedWorkers: selectedWorkers,
                selectedTeams: selectedTeams
            }),
            success: function (response) {
                $('#teamTableContainer').html(response);

                restartTeamsTable()
            },
            error: function (xhr, status, error) {
                console.error('Error updating teams:', error);
            }
        });
    }

    function restartWorkersTable() {
        $('#createShiftWorkerTable').DataTable().destroy();
        //$('#createShiftWorkerTable').dataTable(createShiftWorkerTableConfig);
        $('#createShiftWorkerTable').DataTable({
            scrollY: '400px',
            scrollCollapse: true,
            paging: false,
            searching: true,
            columnDefs: [
                {
                    targets: -1,
                    searchable: false
                }
            ],
            language: {
                "decimal": ",",
                "thousands": ".",
                "lengthMenu": "Mostrar _MENU_ registos por página",
                "zeroRecords": "Nenhum registo encontrado",
                "info": "A mostrar _START_ a _END_ de _TOTAL_ registos",
                "infoEmpty": "A mostrar 0 a 0 de 0 registos",
                "infoFiltered": "(filtrado de _MAX_ registos no total)",
                "search": "Procurar:",
                "paginate": {
                    "first": "Primeiro",
                    "last": "Último",
                    "next": "Seguinte",
                    "previous": "Anterior"
                },
                "emptyTable": "Sem dados disponíveis na tabela",
                "loadingRecords": "A carregar...",
                "processing": "A processar...",
                "aria": {
                    "sortAscending": ": ativar para ordenar a coluna de forma ascendente",
                    "sortDescending": ": ativar para ordenar a coluna de forma descendente"
                }
            }
        });
    }

    function restartTeamsTable() {
        $('#createShiftTeamTable').dataTable().destroy();
        //$('#createShiftTeamTable').dataTable(createShiftTeamTableConfig);
        $('#createShiftTeamTable').dataTable({
            scrollY: '400px',
            scrollCollapse: true,
            paging: false,
            searching: true,
            columnDefs: [
                {
                    targets: -1,
                    searchable: false
                }
            ],
            language: {
                "decimal": ",",
                "thousands": ".",
                "lengthMenu": "Mostrar _MENU_ registos por página",
                "zeroRecords": "Nenhum registo encontrado",
                "info": "A mostrar _START_ a _END_ de _TOTAL_ registos",
                "infoEmpty": "A mostrar 0 a 0 de 0 registos",
                "infoFiltered": "(filtrado de _MAX_ registos no total)",
                "search": "Procurar:",
                "paginate": {
                    "first": "Primeiro",
                    "last": "Último",
                    "next": "Seguinte",
                    "previous": "Anterior"
                },
                "emptyTable": "Sem dados disponíveis na tabela",
                "loadingRecords": "A carregar...",
                "processing": "A processar...",
                "aria": {
                    "sortAscending": ": ativar para ordenar a coluna de forma ascendente",
                    "sortDescending": ": ativar para ordenar a coluna de forma descendente"
                }
            }
        });

    }
});


