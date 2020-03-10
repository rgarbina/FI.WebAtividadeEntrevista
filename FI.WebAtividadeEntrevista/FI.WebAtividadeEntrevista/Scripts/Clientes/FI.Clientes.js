
$(document).ready(function () {
    $(".cpf").mask('999.999.999-99');

    $('#formCadastro').submit(function (e) {
        var isvalid = $("#formCadastro").valid();
        if (isvalid) {
            var cpf = $(this).find("#Cpf").val().replace(/[.-]/g, "");
            e.preventDefault();
            $.ajax({
                url: urlPost,
                method: "POST",
                data: {
                    "NOME": $(this).find("#Nome").val(),
                    "CEP": $(this).find("#CEP").val(),
                    "Email": $(this).find("#Email").val(),
                    "Sobrenome": $(this).find("#Sobrenome").val(),
                    "Nacionalidade": $(this).find("#Nacionalidade").val(),
                    "Estado": $(this).find("#Estado").val(),
                    "Cidade": $(this).find("#Cidade").val(),
                    "Logradouro": $(this).find("#Logradouro").val(),
                    "Telefone": $(this).find("#Telefone").val(),
                    "CPF": cpf,
                    "Benficiarios": GetBeneficiariosTable()
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
                        ModalDialog("Sucesso!", r);
                        LimparForm();
                        $("#formCadastro")[0].reset();
                        $('#gridBeneficiários tbody tr').remove();
                    }
            });
        }
    });
    $("#formCadastro").validate({
        rules: {
            cpf: { cpf: true, required: true }
        },
        messages: {
            cpf: { cpf: 'CPF inválido' }
        }
    });

    $('#btnModalBenificio').click(function (e) {
        e.preventDefault();
        LimparForm();

        $("#modalAbatimento").modal("show");
    });
});
jQuery.validator.addMethod("cpf", function (value, element) {
    return this.optional(element) || validateCPF(value);
}, "Informe um CPF válido");

function ModalDialog(titulo, texto) {
    var random = Math.random().toString().replace('.', '');
    var html = '<div id="' + random + '" class="modal fade">                                                               ' +
        '        <div class="modal-dialog">                                                                                 ' +
        '            <div class="modal-content">                                                                            ' +
        '                <div class="modal-header">                                                                         ' +
        '                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>         ' +
        '                    <h4 class="modal-title">' + titulo + '</h4>                                                    ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-body">                                                                           ' +
        '                    <p>' + texto + '</p>                                                                           ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-footer">                                                                         ' +
        '                    <button type="button" class="btn btn-default" data-dismiss="modal">Fechar</button>             ' +
        '                                                                                                                   ' +
        '                </div>                                                                                             ' +
        '            </div><!-- /.modal-content -->                                                                         ' +
        '  </div><!-- /.modal-dialog -->                                                                                    ' +
        '</div> <!-- /.modal -->                                                                                        ';

    $('body').append(html);
    $('#' + random).modal('show');
}
