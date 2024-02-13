function loadGrid(data) {
    // new DataTable(id_table);

    $('#datagrid').twbsDatagrid({
        url : 'data.json',		// request url
        columns : data.Columns,
        datas : data.Rows
    });
}