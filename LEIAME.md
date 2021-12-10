

Informações           |Descrição
----------------------|---------------------
Linguagem             |C# .net Core App 3.1
Saída                 |Executável (exe)
Tipo                  |Standalone
Runtime Destino       |Win-x64
Parâmetro de execução |String Caminho

# Descrição

O aplicativo tem como objetivo ajustar quatro parâmetros dos arquivos web.config:

- OfflineTimeEventsDirectory;
- UserSessions;
- UserStateDirectory;
- UserSessionsDirectory.

Caso algum dos parâmetros esteja sem o nome do cliente em questão:

1. Será atualizado o registro colocando o nome que se encontra após o APW e antes da primeira barra após esse termo;
1. Será gerado uma pasta Log na pasta de execução, juntamente com o arquivo com as alterações, contemplando os registros anteriores e atuais, após o ajuste;
1. Antes da finalização, será criado um backup do arquivo com o formato de WEB.CONFIG\_AAAAMMDD\_HHMM no mesmo diretório do web.config em questão.

# Como utilizar

Para utilizar, basta colocar o MaurisBot.exe no diretório desejável e executá-lo passando o parâmetro do caminho do diretório que deseja escanear a partir.

exemplo de chamada:

>Maurisbot.exe D:\Portais\

# Precauções

O aplicativo não funcionará/corrigirá caso:
- Seja passado um diretório raiz com pastas bloqueadas (os discos C:\ D:\ raiz costumam ter diretórios com acesso negado);
- Os diretórios não estejam no formato padrão (por exemplo, em negrito, D:\Portais\\\*\*C###\_APW\_NOMECLIENTE\Server\ApWebDispatcher\\.net\web.config\*\*);
- Caso algum dos parâmetros não exista ou esteja vazio, o aplicativo ignorará (não criará o caminho para o parâmetro em específico);

⚠️**Atenção**: Tenha certeza de onde precisa que seja escaneado e alterado, pois o aplicativo salva o arquivo substituindo o anterior pelo ajustado.
