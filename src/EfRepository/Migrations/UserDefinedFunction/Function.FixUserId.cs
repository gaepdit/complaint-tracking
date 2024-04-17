namespace Cts.EfRepository.Migrations.UserDefinedFunction;

public static class Function
{
    // language=sql
    public const string FixUserId =
        """
        CREATE OR ALTER FUNCTION dbo.FixUserId(@input nvarchar(450)) RETURNS nvarchar(450) AS
        BEGIN
            RETURN
                case
                    when @input = '00000000-0000-0000-0000-000000000000' then null
                    when @input = '9311E9DE-C35F-4CF4-8579-800E1B51D998' then 'dee30bf0-5ec3-4065-9f8d-07338559477b'
                    when @input = 'A99E986A-F9FD-4562-A4C1-DF93FDC1B0B0' then '7bf9013e-b422-46fe-b72b-8ef5ce5466bd'
                    when @input = '509E3F5B-CED2-4684-B96D-0D2E6D19990B' then '731a6089-bd34-4b93-801b-9ed292627883'
                    when @input = '727F3763-26E7-4795-8380-FA33E8DBF617' then '731a6089-bd34-4b93-801b-9ed292627883'
                    when @input = '62AC545A-0F18-465E-B02C-9BBFA88EB8F0' then '68d5cc0d-0019-45f5-bd89-3615da6b659d'
                    when @input = '90F5B007-F33A-4CF9-A187-0BF0D9385C77' then '5df4243d-00c4-409b-a38d-5188f3dfeb73'
                    when @input = '57949120-5566-4192-899B-5684A5CC9793' then '5df4243d-00c4-409b-a38d-5188f3dfeb73'
                    when @input = '85B517B0-E5A7-4ADF-9381-41CB2BDBF168' then '91de2416-e79b-431d-b8bf-22687367100d'
                    when @input = '21AED5D9-12FA-46DB-BFA1-049BD6734E42' then '11a24f44-d621-4598-b20e-b1001ef0ea7c'
                    when @input = '7C7485D1-840E-4172-B30A-52183C48233E' then '1e5b17dd-11cd-4e6b-beb2-84cfa936fa19'
                    when @input = 'C339ECFC-69D1-43D4-B730-0C064BAA1589' then '1f0fc871-5b0c-4653-a1e2-c31ab5a96f38'
                    when @input = '5ABA11B5-A0CF-4B05-97A3-07712A873D7D' then '27a136cb-d02f-4ee9-a116-93f98e241d30'
                    when @input = '0BE87595-6B51-4154-8BAD-77885834CA38' then 'a1267d03-89d5-44e8-a30f-7b8c44c746e6'
                    when @input = '081F3450-4B74-4051-8F3F-DCF31B5559B9' then '8efa5b08-e092-49cb-bb1b-7de5cd49ad98'
                    when @input = 'EAE3ECC3-F259-40F6-996D-7470080DB6BC' then '0EB8BACC-1D14-4FBC-814A-784690EFFC50'
                    when @input = 'AE3F24BF-9096-4459-B015-9F27FDB6B550' then 'e64d6867-bdb0-4c35-9cb8-e69fe484060f'
                    when @input = '61F069FF-36C8-4EB0-88B4-60A5C7D4AD4C' then '268c8385-f87f-44a2-b3cf-e9e5ca7cccb1'
                    when @input = 'FB8EE435-EBD3-4E59-BA60-6032C282D964' then '37a9226e-09fd-43b5-9f3f-0fbe9c218ade'
                    when @input = 'AFDE9820-2FEB-4351-B65B-067F58BADF19' then 'cf53dbbd-d7fa-442f-8154-23ba750c56eb'
                    when @input = 'B2FC5031-E6CC-4F4B-911D-7B767B00ADAE' then '963d706a-889f-4aeb-8914-037f2c11d0bf'
                    when @input = 'DBBC470F-D60C-41A4-ACC6-6761984151F9' then 'ee51381f-52df-41eb-90b2-16d8809e55a4'
                    when @input = 'B950DC18-8278-4B0A-81D4-6298561C86B7' then '481d2e31-3df9-4f74-8aaa-a7a26886dcf1'
                    when @input = '77B5063F-E590-46BC-B3F1-90A75A5F7AC7' then 'af535f81-5968-418e-8150-04548ec9d06b'
                    when @input = 'E734F906-FCAE-43A3-806A-55C0DF4B1033' then 'de35e685-dc70-490c-855b-5d78defa2a11'
                    when @input = '7BFAFD02-389D-41E4-8F83-B1CCEBB6FF51' then '592a8173-ac8a-454e-959b-23304775ec62'
                    else lower(@input)
                end;
        END 
        """;

    // language=sql
    public const string DropFixUserId = "DROP FUNCTION IF EXISTS dbo.FixUserId;";
}
