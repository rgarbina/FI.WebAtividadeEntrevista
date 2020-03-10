$(document).ready(function () {
    $('#btnIncluirBenificio').click(function (e) {
        e.preventDefault();
        var invalido = false;
        var msg = "";

        if (!validateCPF($('#CpfBeneficiario').val())) {
            msg += "CPF inválido!";
            invalido = true;
        }
        if ($('#NomeBeneficiario').val() == '') {
            msg += "\nÉ necessario digitar nome.";
            invalido = true;
        }

        if (invalido) {
            alert(msg);
        }
        else {
            IncluiBeneficiarioTabela();
        }
    });
});

function ListBeneficiarios() {
    var url = window.location.pathname;
    var id = url.substring(url.lastIndexOf('/') + 1);
    $.ajax({
        url: urlListBenef,
        method: "GET",
        data: {
            "IDCLIENTE": id
        },
        error:
            function (r) {
                if (r.status == 400)
                    ModalDialog("Ocorreu um erro", r.responseJSON);
                else if (r.status == 500)
                    ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
            },
        success:
            function (r) {
                $.each(r, function (index, value) {
                    AppendGridBeneficiarios(value.CPF, value.Nome);
                });
                $("#modalAbatimento").modal("show");
            }
    });
}

function GetBeneficiariosTable() {
    var beneficiarios = [];
    $('#gridBeneficiários tbody tr').each(function (index, tr) {
        var row = $(tr).find("td");
        var cpfBeneficiario = row.eq(0).html().replace(/[.-]/g, "");
        var nomeBeneficiario = row.eq(1);
        beneficiarios.push({
            CPF: cpfBeneficiario,
            Nome: nomeBeneficiario.html()
        });
    });
    return beneficiarios;
}

function RemoveBeneficiarioTabela(e) {
    $(e).closest('tr').remove();
}

function AlterarBeneficiarioTabela(e) {
    var row = $(e).closest('tr').find("td");
    var cpfBeneficiario = row.eq(0);
    var nomeBeneficiario = row.eq(1);

    $('#IdBeneficiarioTabela').val($(e).closest('tr').index());
    $('#CpfBeneficiario').val(cpfBeneficiario.html());
    $('#NomeBeneficiario').val(nomeBeneficiario.html());
}

function IncluiBeneficiarioTabela() {
    var nomeBeneficiario = $('#NomeBeneficiario').val();
    var cpfBeneficiario = $('#CpfBeneficiario').val();
    var idBenificiario = $('#IdBeneficiarioTabela').val();
    var existeCpfGridBenificiario = $('#gridBeneficiários tr').is('#' + cpfBeneficiario);
    if (nomeBeneficiario !== '' && cpfBeneficiario !== '') {
        if (idBenificiario || existeCpfGridBenificiario) {
            var index = idBenificiario;
            if (!index) {
                index = $('#gridBeneficiários tr#' + cpfBeneficiario).index();
            }
            var tr = $('#gridBeneficiários tbody tr').eq(index);
            var row = $(tr).find('td');
            tr.attr("id", cpfBeneficiario);
            row.eq(0).html(cpfBeneficiario);
            row.eq(1).html(nomeBeneficiario);
        }
        else {
            AppendGridBeneficiarios(cpfBeneficiario, nomeBeneficiario);
        }
        LimparForm();
    }
}

function AppendGridBeneficiarios(cpf, nome) {
    cpf = cpf.replace(/^(\d{3})(\d{3})(\d{3})(\d{2}).*/, '$1.$2.$3-$4');
    var html = `<tr id="${cpf}">
                    <td class="cpf">${cpf}</td>
                    <td>${nome}</td>
                    <td>
                        <input type="button" class="btn btn-sm btn-primary btnAlterarBenificio" value="Alterar" onClick="AlterarBeneficiarioTabela(this)">
                        <input type="button" class="btn btn-sm btn-primary btnExcluirBenificio" value="Excluir" onClick="RemoveBeneficiarioTabela(this)">
                    </td>
                </tr>`;
    $('#gridBeneficiários').append(html);
}

function LimparForm() {
    $('#NomeBeneficiario').val('');
    $('#IdBeneficiarioTabela').val('');
    $('#CpfBeneficiario').val('');
}