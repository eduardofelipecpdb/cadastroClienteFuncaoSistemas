/// <summary>
/// Sistema desenvolvido para processo seletivo função sistemas
/// Autor: Eduardo Felipe de Souza
/// Antes de executar limpe o cache do seu navegador com CTRL + F5
/// </summary>
$(document).ready(function () {
    $('#formCadastro').submit(function (e) {
        var TableData = new Array();

        $('#table tr').each(function (row, tr) {
            TableData[row] = {
                "CPFBeneficiario": $(tr).find('td:eq(0)').text(),
                "NomeBeneficiario": $(tr).find('td:eq(1)').text()
            }
        });
        TableData.shift();

        e.preventDefault();
        $.ajax({
            url: urlPost,
            method: "POST",
            data: {
                "beneficiarios": TableData,
                "CPF": $(this).find("#CPF").val(),
                "NOME": $(this).find("#Nome").val(),
                "CEP": $(this).find("#CEP").val(),
                "Email": $(this).find("#Email").val(),
                "Sobrenome": $(this).find("#Sobrenome").val(),
                "Nacionalidade": $(this).find("#Nacionalidade").val(),
                "Estado": $(this).find("#Estado").val(),
                "Cidade": $(this).find("#Cidade").val(),
                "Logradouro": $(this).find("#Logradouro").val(),
                "Telefone": $(this).find("#Telefone").val()
            },
            error:
                function (r) {
                    if (r.status == 400)
                        ModalDialog("Ocorreu um erro", r.responseJSON)
                    else if (r.status == 500)
                        ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.")
                },
            success:
                function (r) {
                    ModalDialog("Sucesso!", r)
                    $("#formCadastro")[0].reset();
                }
        });

    });

    $('#Beneficiarios').on('click', function (e) {
        $('#modalBeneficiario').modal('show');
    });

    $('#incluirbeneficiario').on('click', function (e) {
        if ($('#cpfBeneficiario').val() == "" || $('#nomeBeneficiario').val() == "") {
            alert("Preencha nome e CPF do beneficiário");
        }
        else {
            $('#tableBeneficiarios').append("<tr><td class='cpf'>" + $('#cpfBeneficiario').val() + "</td><td class='nome'>" + $('#nomeBeneficiario').val() + "</td><td><span class='table-edit'><button type='button' class='btn btn-warning'>Editar</button></span>     <span class='table-remove'><button type='button' class='btn btn-danger'>Excluir</button></span></td></tr>");
        }
    });

    $('#table').on('click', '.table-remove', function () {
        $(this).parents('tr').detach();
    });

    $('#table').on('click', '.table-edit', function () {
        $('#cpfBeneficiario').val($(this).parents('tr').find('.cpf').text());
        $('#nomeBeneficiario').val($(this).parents('tr').find('.nome').text());
        $(this).parents('tr').detach();
    });
});

function ModalDialog(titulo, texto) {
    var random = Math.random().toString().replace('.', '');
    var texto = '<div id="' + random + '" class="modal fade">                                                               ' +
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

    $('body').append(texto);
    $('#' + random).modal('show');
}