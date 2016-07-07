# SdoValidering
Dette kodeeksempel viser hvordan esignaturs Signeringsbevis.xml fil kan valideres.

Venligst bemærk, at signeringsbeviset er indlejret i Object-noden i et standard XMLDSIG (SignedXML) dokument.

Eksemplet validerer XMLDSIG dokumentet i forhold til esignaturs signing certifikat (CN=www.esignatur.dk) og validerer endvidere også certifikat kæden.

Herefter kan det signeringsbeviset afkobles som et selvstænding dokument fra Object-noden (ikke vist i koden).

Et esignatur Signeringsbevis version 1 dokument har følgende struktur:

```
<version />              <!-- versions nummer, p.t. v1.0 -->
<dtd />                  <!-- URI til DTD -->
<comment />              <!-- Læs-let kommentar til læseren -->
<hashSum>                <!-- Det indlejrede dokuments fingeraftryk -->
  <algorithm />          <!-- Altid SHA512 for v1.0 -->
  <sum />                <!-- BASE64 kodet sum -->
</hashSum>
<originalDocument />     <!-- BASE64 kodet dokument - det der underskrives -->
<referenceId />          <!-- esignatur ordre reference ID -->
<agreementId />          <!-- Dokumentets aftale ID - er også skrevet i margin på det underskrevne dokument -->
<sealDate />             <!-- Forseglingsdato (dato + tid) -->
<signers>                <!-- Alle underskrivere af dette dokument -->
  <signer>               <!-- Definition på en underskriver - der kan være flere af disse -->
    <name />             <!-- Underskrivers fulde navn (taget fra NemID certifikatet) -->
    <netsId />           <!-- NemID identifikation -->
    <signedDate />       <!-- Dato for underskrift -->
    <onBehalfOf />       <!-- Hvis underskriver underskriver på vegne af noteres hvem -->
    <sdo>                <!-- Signaturfil fra Nets -->
      <provider />       <!-- Altid nemid for v1.0 -->
      <type />           <!-- NemID type (MOCES, POCES, ...) -->
      <data />           <!-- Signaturfil, BASE64 kodet -->
    </sdo>
    <roles>              <!-- Hvis underskriver underskriver i en eller flere roller noteres hvilke -->
      <role />           <!-- Rolle (der kan være flere af disse) -->
    </roles>
  </signer>
</signers>
<documentsInOrder>       <!-- Hvis der er flere dokumenter i ordren noteres hvilke -->
  <document />           <!-- Aftale ID på dokument (der kan være flere af disse) -->
</documentsInOrder>
