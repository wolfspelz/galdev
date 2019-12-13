using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace galdevtool.Test
{
    [TestClass]
    public class Bigfile2YamlTest
    {
        [TestMethod]
        public void Analyze_complete()
        {
            var data = "2052	Personenrechte für Primaten # Als erstes Land beschließt Island, Primaten als Personen anzuerkennen. Einige, speziell benannte Primatenarten werden rechtlich Kleinkindern gleichgestellt. Damit erhalten sie theoretisch die Menschenrechte, vor allem Persönlichkeitsrechte (Leben, körperliche Unversehrtheit) und Freiheitsrechte. Wenn nötig, bekommen sie einen Vormund, der ihre Rechte vertritt. Praktisch müssen ihre Rechte in Einzelfällen vor Gericht erstritten werden. Aber wenn ein Vormund entscheidet, dass ein bestimmter Schimpanse nicht im Käfig gehalten werden soll, dann stehen die Chancen gut, dass ein Gericht dem Antrag stattgibt, basierend auf dem Selbstbestimmungsrecht. # Das Gesetz ist sehr weitreichend. Mit der neuen Einordnung von Primaten werden auch andere Tiere aufgewertet. Einige Arten mit relativ hohen kognitiven Fähigkeiten werden zwar nicht als Personen, aber immerhin als nichtmenschliche Wesen eingeordnet. Darunter sind Oktopus, Elefant, Delfin, einige Wale, Wolf, Hund, und einige Haustiere. Diese Tiere werden nun nicht mehr als 'Sache' betrachtet, die immer einem Menschen gehört, sondern als 'Wesen', einer neuen Klasse rechtlicher Entitäten neben dem Menschen und den 'Sachen'. Damit sind Individuen dieser Arten nicht mehr automatisch das Eigentum von Menschen oder juristischen Personen. # Eine dauerhafte Kommission wird eingerichtet, um über die Auswahl der Arten zu entscheiden. Die Kommission ist besetzt mit Vertretern vieler gesellschaftlicher Gruppen. Sie bewertet die Selbsterkenntnis, soziales Verhalten, Planungsfähigkeiten, die Fähigkeit zu sprechen (durch Laute oder Zeichensprache), Ursache und Wirkung zu erkennen, Innovation, Imitation und Lernverhalten. Das Althing (das isländische Parlament) gibt explizit vor, dass in der Diskussion der Kommission nur neue Erkenntnisse verwendet werden dürfen, die nach der wissenschaftlichen Methode (Beobachtung-Hypothese-Test) gewonnen werden. Explizit ausgeschlossen werden damit traditionelle und kommerzielle Argumente, also 'das war schon immer so', bzw. 'das steht in alten Schriften' oder 'das können wir uns nicht leisten'. Im Jahr 2062 kommen – gegen den Widerstand der Lebensmittelindustrie – auch Nutztiere dazu. Damit wird die Haltung von Schweinen zur Fleischproduktion praktisch illegal. # 'Wie sollen wir mit Wesen umgehen, die sich im Spiegel erkennen, die um Gefährten trauern, die ein Bewusstsein für sich als Individuum haben? Verdienen sie es nicht, dass wir sie so behandeln wie andere, genauso empfindsame Wesen: uns selbst?' - Jane Goodall. # Es war ein langer Weg vom Verbot von Pelzfarmen, Legebatterien und Tierversuchen für Kosmetika am Anfang des 21. Jahrhunderts bis zur ersten vollen Anerkennung als Person durch einen souveränen Staat. Auch vorher gab es schon Einzelfälle in denen Tieren Rechte zugesprochen wurden. In Indien sind Tiere schon lang als 'Wesen' (im Gegensatz zu 'Sachen') geschützt. Auch in Deutschland hatten Tiere Rechte, die allerdings in der Praxis oft hinter kommerziellen Argumenten zurücktreten mussten. In den 20er und 30er Jahren bekam die Bewegung für Tierrechte (NHR, non human rights movement) immer mehr Zulauf. Skandinavien, Neuseeland, Bhutan und Ecuador beschlossen Rechte für Tiere, sowie Ausbeutungs- und Misshandlungsverbote. In den 40er Jahren kamen viele Länder und selbstverwaltete Regionen dazu. # Schließlich setzt sich die Erkenntnis durch, dass die Menschlichkeit keine entweder/oder Entscheidung ist, sondern dass es ein Kontinuum gibt in den kognitiven und sozialen Fähigkeiten, dass Schimpansen genauso intelligent, selbstbewusst und mitfühlend sind wie Menschenkinder, dass wir auch Menschen mit eingeschränkten Fähigkeiten nicht die Menschlichkeit absprechen und dass es dem Menschen nicht wirklich zum Schaden gereicht, wenn er Mitwesen genauso schützt, wie schützenswerte Mitmenschen. # Nach Island folgen schnell andere skandinavische und dann europäische Staaten mit der vollen Anerkennung von Personenrechten für Primaten. # In den folgenden 20 Jahren schließen sich weitere 50 Staaten der Erde an. Bezogen auf die (menschliche) Bevölkerung sind das allerdings nur 15%. # Orang-Utans werden die Menschenrechte posthum zuerkannt. | tags=#Menschenrechte #Primaten #Personen #Tiere #Delfine #Wale | image=Primaten.jpg | twitter=2052 Personenrechte für Primaten http://www.galactic-developments.de/data2.html?hilite=2052 #scifi #Menschenrechte #Primaten #Personen #Tiere #Delfine #Wale | twitterimage=Primaten-snpost.jpg | facebook=Unsere Brüder und Schwestern: Mitwesen der Menschen - 2052 Personenrechte für Primaten. Immer mehr Staaten erkennen die großen Menschenaffen als Personen an und gewähren ihnen Menschenrechte oder vergleichbare Rechte... Weiter: http://jmp1.de/h2052 # 2052 Personenrechte für Primaten # Immer mehr Staaten erkennen die großen Menschenaffen als Personen an und gewähren ihnen Menschenrechte oder vergleichbare Rechte... Weiter: http://jmp1.de/h2052 | facebookimage=Primaten-snpost.jpg | topic=ecology".Replace("'", "\"");
            var entries = new Bigfile2Yaml().Analyse(data);
            Assert.AreEqual(1, entries.Count);
            var e = entries[0];
            Assert.AreEqual("Primaten", e.Name);
            Assert.AreEqual("2052", e.Year);
            Assert.AreEqual("Personenrechte für Primaten", e.Title);
            Assert.AreEqual("Immer mehr Staaten erkennen die großen Menschenaffen als Personen an und gewähren ihnen Menschenrechte oder vergleichbare Rechte...", e.Short);
            Assert.AreEqual("", e.Summary);
            Assert.AreEqual("Unsere Brüder und Schwestern: Mitwesen der Menschen", e.Headline);
            Assert.AreEqual("Primaten.jpg", e.Image);
            Assert.AreEqual("", e.Smallimage);
            Assert.AreEqual(6, e.Tags.Count);
            Assert.AreEqual("2052 Personenrechte für Primaten. Immer mehr Staaten erkennen die großen Menschenaffen als Personen an und gewähren ihnen Menschenrechte oder vergleichbare Rechte... Weiter: http://jmp1.de/h2052", e.Post);
            Assert.AreEqual("Primaten-snpost.jpg", e.Postimage);
            Assert.AreEqual("2052 Personenrechte für Primaten http://www.galactic-developments.de/data2.html?hilite=2052 #scifi #Menschenrechte #Primaten #Personen #Tiere #Delfine #Wale", e.Twitter);
            Assert.AreEqual("Primaten-snpost.jpg", e.Twitterimage);
            Assert.AreEqual("2052 Personenrechte für Primaten. Immer mehr Staaten erkennen die großen Menschenaffen als Personen an und gewähren ihnen Menschenrechte oder vergleichbare Rechte... Weiter: http://jmp1.de/h2052", e.Facebook);
            Assert.AreEqual("Primaten-snpost.jpg", e.Facebookimage);
            Assert.AreEqual(1, e.Topics.Count);
            Assert.AreEqual(9, e.Text.Count);
        }

        [TestMethod]
        public void Analyze_title_only()
        {
            var data = "2060	100 Menschen leben und arbeiten permanent im Orbit und auf dem Mond.".Replace("'", "\"");
            var entries = new Bigfile2Yaml().Analyse(data);
            Assert.AreEqual(1, entries.Count);
            var e = entries[0];
            Assert.AreEqual("", e.Name);
            Assert.AreEqual("2060", e.Year);
            Assert.AreEqual("100 Menschen leben und arbeiten permanent im Orbit und auf dem Mond", e.Title);
            Assert.AreEqual("", e.Short);
            Assert.AreEqual("", e.Summary);
            Assert.AreEqual("", e.Headline);
            Assert.AreEqual("", e.Image);
            Assert.AreEqual("", e.Smallimage);
            Assert.AreEqual(0, e.Tags.Count);
            Assert.AreEqual("", e.Post);
            Assert.AreEqual("", e.Postimage);
            Assert.AreEqual("", e.Twitter);
            Assert.AreEqual("", e.Twitterimage);
            Assert.AreEqual("", e.Facebook);
            Assert.AreEqual("", e.Facebookimage);
            Assert.AreEqual(0, e.Topics.Count);
            Assert.AreEqual(0, e.Text.Count);
        }

        [TestMethod]
        public void Analyze_title_and_1_paragraph_only()
        {
            var data = "2136	Über 1000 Menschen leben und arbeiten im interplanetaren Raum. # Fast die Hälfte davon arbeitet an SCALE, im Erdorbit, auf dem Mond und am L1 Lagrange-Punkt.".Replace("'", "\"");
            var entries = new Bigfile2Yaml().Analyse(data);
            Assert.AreEqual(1, entries.Count);
            var e = entries[0];
            Assert.AreEqual("", e.Name);
            Assert.AreEqual("2136", e.Year);
            Assert.AreEqual("Über 1000 Menschen leben und arbeiten im interplanetaren Raum", e.Title);
            Assert.AreEqual("", e.Short);
            Assert.AreEqual("", e.Summary);
            Assert.AreEqual("", e.Headline);
            Assert.AreEqual("", e.Image);
            Assert.AreEqual("", e.Smallimage);
            Assert.AreEqual(0, e.Tags.Count);
            Assert.AreEqual("", e.Post);
            Assert.AreEqual("", e.Postimage);
            Assert.AreEqual("", e.Twitter);
            Assert.AreEqual("", e.Twitterimage);
            Assert.AreEqual("", e.Facebook);
            Assert.AreEqual("", e.Facebookimage);
            Assert.AreEqual(0, e.Topics.Count);
            Assert.AreEqual(1, e.Text.Count);
            Assert.AreEqual("Fast die Hälfte davon arbeitet an SCALE, im Erdorbit, auf dem Mond und am L1 Lagrange-Punkt.", e.Text[0]);
        }

        [TestMethod]
        public void Analyze_title_text_topic_only()
        {
            var data = "2153	Eter, ein neues Paradigma in der Datenverarbeitung revolutioniert die Informationstechnologie. # Während der traditionelle Ansatz auf Rechenknoten und Datenpaketen basierte, führt der wachsende Einsatz von Eter-Technologien zu einer weitgehend delokalisierten und kollektiven Leistungserbringung. # Obwohl Ausfallsicherheit das wesentliche Designziel des Internets im späten 20. Jahrhundert war, wurden im 21. Jahrhundert mit Internettechnologien vor allem hierarchische und zentralisierte Systeme geschaffen. Aus Sicht der Anwender ist Informationsverarbeitung zwar allgegenwärtig verfügbar. Aber selbst Grid- und Cloud-Architekturen sind nur scheinbar verteilt. Letztlich werden Verarbeitungs- und Speicherleistungen in Rechenzentren erbracht und über Netzknoten zu Anwendern geleitet. Das ändert sich nun. Delokalisierung macht die Netze immun gegen den Ausfall einzelner Knoten. Datenverarbeitung wird nicht mehr von dedizierten Verarbeitungseinheiten mit entsprechender Software erbracht und Informationen sind nicht mehr an mehreren Stellen 'redundant gespeichert', sondern delokalisiert. 'Interferenz' und 'Flow' ersetzen Begriffe wie 'Server' und 'Backbone'. # Norware, ein IT-Konglomerat aus Skandinavien, das nach Yksityis-Grundsätzen geführt wird, ist dabei weltweit führend. | topic=technology".Replace("'", "\"");
            var entries = new Bigfile2Yaml().Analyse(data);
            Assert.AreEqual(1, entries.Count);
            var e = entries[0];
            Assert.AreEqual("", e.Name);
            Assert.AreEqual("2153", e.Year);
            Assert.AreEqual("Eter, ein neues Paradigma in der Datenverarbeitung revolutioniert die Informationstechnologie", e.Title);
            Assert.AreEqual("", e.Short);
            Assert.AreEqual("", e.Summary);
            Assert.AreEqual("", e.Headline);
            Assert.AreEqual("", e.Image);
            Assert.AreEqual("", e.Smallimage);
            Assert.AreEqual(0, e.Tags.Count);
            Assert.AreEqual("", e.Post);
            Assert.AreEqual("", e.Postimage);
            Assert.AreEqual("", e.Twitter);
            Assert.AreEqual("", e.Twitterimage);
            Assert.AreEqual("", e.Facebook);
            Assert.AreEqual("", e.Facebookimage);
            Assert.AreEqual("technology", e.Topics[0]);
            Assert.AreEqual(3, e.Text.Count);
            Assert.AreEqual("Während der traditionelle Ansatz auf Rechenknoten und Datenpaketen basierte, führt der wachsende Einsatz von Eter-Technologien zu einer weitgehend delokalisierten und kollektiven Leistungserbringung.".Replace("'", "\""), e.Text[0]);
            Assert.AreEqual("Obwohl Ausfallsicherheit das wesentliche Designziel des Internets im späten 20. Jahrhundert war, wurden im 21. Jahrhundert mit Internettechnologien vor allem hierarchische und zentralisierte Systeme geschaffen. Aus Sicht der Anwender ist Informationsverarbeitung zwar allgegenwärtig verfügbar. Aber selbst Grid- und Cloud-Architekturen sind nur scheinbar verteilt. Letztlich werden Verarbeitungs- und Speicherleistungen in Rechenzentren erbracht und über Netzknoten zu Anwendern geleitet. Das ändert sich nun. Delokalisierung macht die Netze immun gegen den Ausfall einzelner Knoten. Datenverarbeitung wird nicht mehr von dedizierten Verarbeitungseinheiten mit entsprechender Software erbracht und Informationen sind nicht mehr an mehreren Stellen 'redundant gespeichert', sondern delokalisiert. 'Interferenz' und 'Flow' ersetzen Begriffe wie 'Server' und 'Backbone'.".Replace("'", "\""), e.Text[1]);
            Assert.AreEqual("Norware, ein IT-Konglomerat aus Skandinavien, das nach Yksityis-Grundsätzen geführt wird, ist dabei weltweit führend.".Replace("'", "\""), e.Text[2]);
        }

        [TestMethod]
        public void Analyze_Bigbang()
        {
            var data = "2410	Urknall-Theorie widerlegt. # Astronomische Präzisionsmessungen des Giant Gravitic Interferometer Arrays bestätigen die CBM Theorie der Kosmologie (Communicating Bubble Multiverse). Damit wird die kosmische Inflation erklärt und die QLC Urknall-Theorie widerlegt. # Im Spektrum von akustischen Resonanzen im Gravitationswellenhintergrund findet sich der Beweis dafür, dass unser Universum die Hülle einer 4-dimensionalen Blase ist, die von anderen Universen mit Raumzeit angefüllt wird. Der aktuelle Durchmesser unseres Universums ist 50 Millionen Mal größer, als der sichtbare Bereich. Damit beträgt das Volumen des Universums annähernd 10^56 Kubik Lichtjahre. # Die Daten zeigen eine Asymmetrie, die auf eine 4-dimensionale Öffnung zu anderen Universen hinweist. Wir befinden uns nur etwa 100 Peta-Lichtjahre (10^17) von der Öffnung entfernt, also 'relativ nahe'. Trotzdem ist die Öffnung natürlich unerreichbar. Sie ist drei Millionen Durchmesser unseres sichtbaren Bereichs entfernt. # Die Inflation wurde hervorgerufen durch einen besonders schnellen initialen Raumzeit-Strom. Die Frage bleibt, warum die Raumzeit von einem Universum zum anderen fließt und wie schnell sie wieder abfließen kann. Da die Inflation nur 10^-30 Sekunden dauerte, liegt die Vermutung nahe, dass eine mögliche kosmische Deflation genauso schnell gehen könnte, d.h. das Multiversum kann unserem Universum seine Raumzeit von einem Moment auf den anderen entziehen. | tags=#Kosmologie #Astronomie #Wissenschaft #Teleskop  | image=BigBang.jpg | twitterimage=BigBang.jpg | facebook=Urknall-Theorie widerlegt http://jmp1.de/h2410 # 2410 Urknall-Theorie widerlegt. Astronomische Präzisionsmessungen des Giant Gravitic Interferometer Arrays bestätigen die Communicating Bubble Multiverse Theorie der Kosmologie. Damit wird die kosmische Inflation erklärt und die Urknall-Theorie widerlegt. | facebookimage=BigBang.jpg | topic=science".Replace("'", "\"");
            var entries = new Bigfile2Yaml().Analyse(data);
            Assert.AreEqual(1, entries.Count);
            var e = entries[0];
            Assert.AreEqual("BigBang", e.Name);
            Assert.AreEqual("2410", e.Year);
            Assert.AreEqual("Urknall-Theorie widerlegt", e.Title);
            Assert.AreEqual("", e.Short);
            Assert.AreEqual("", e.Summary);
            Assert.AreEqual("", e.Headline);
            Assert.AreEqual("BigBang.jpg", e.Image);
            Assert.AreEqual("", e.Smallimage);
            Assert.AreEqual("Urknall-Theorie widerlegt http://jmp1.de/h2410", e.Post);
            Assert.AreEqual("BigBang.jpg", e.Postimage);
            Assert.AreEqual("", e.Twitter);
            Assert.AreEqual("BigBang.jpg", e.Twitterimage);
            Assert.AreEqual("Urknall-Theorie widerlegt http://jmp1.de/h2410", e.Facebook);
            Assert.AreEqual("BigBang.jpg", e.Facebookimage);
            Assert.AreEqual(4, e.Tags.Count);
            Assert.AreEqual(1, e.Topics.Count);
            Assert.AreEqual("science", e.Topics[0]);
            Assert.AreEqual(4, e.Text.Count);
        }

        [TestMethod]
        public void Analyze_Solberg()
        {
            var data = "2681	Entdeckung einer uralten Roboterzivilisation auf dem Planeten Solberg 86 III. # Eine Expedition erreicht das System des Sterns Solberg 86. Die Wissenschaftler gehen in einem Orbit um den dritten Planeten. Dort finden sie eine Roboterzivilisation. Die Zivilisation ist außerordentlich stabil und langlebig. Die Roboter bewachen ein uraltes Erbe ihrer Erbauer. Deshalb sollen sie studiert werden. # Die Expedition hatte das System gezielt angeflogen. Viele Entdeckungen des Aufbruchs werden aufgrund von Recherchen in extrasolaren Bibliotheken gemacht. Die interstellare Raumfahrttechnologie der Menschen ist noch nicht so weit, dass viele Schiffe vom Solsystem ausschwärmen und unbekannte Systeme durchsuchen können. Interstellare Forschungsreisen sind immer noch aufwändig und teuer. Deshalb ist es effizienter in extrasolaren Bibliotheken nach lohnenden Forschungszielen zu recherchieren und diese dann gezielt anzufliegen. Diese Art der 'geplanten Entdeckung' hat sich inzwischen etabliert. Viele interessante Ziele sind bekannt. Aber ein Planet gilt erst dann als von Menschen 'entdeckt', wenn er wirklich von Menschen besucht worden ist. # Für die Recherche haben sich öffentliche Bibliotheken im relativ nahen Thoris-System bewährt. Die Menschheit hat traditionell eine gute Beziehung zur Babur-III-Harmonie im äußeren System. Im Thoris-System gibt es mehrere Handelsvertretungen von Menschen. Diese Stützpunkte haben Zugang zum öffentlichen Netz und zu Bibliotheken. Bei der Recherche über neue Handelsmöglichkeiten fallen den beteiligten Wissenschaftlern immer wieder besondere Systeme der näheren Umgebung auf. Manchmal entstehen so neben der kommerziellen Recherche auch private wissenschaftliche Abhandlungen. Die Information wandert dann über die Mailboxen der Frachter bis zum Solsystem. # Ähnliches gilt für Artu. Allerdings ist die Kommunikation mit den verschiedenen Sophonten im Artu-System wesentlich schwieriger. Schon früh konnte ein reger Handel mit Artu etabliert werden. Aber darüber hinaus sind die Kontakte gering. # Seit der Kelrec-Bedrohung 30 Jahre zuvor besteht außerdem enger Kontakt mit Kisor. Die kompatible Denkweise macht die Kommunikation mit kisorischen Quellen besonders einfach. Zudem gibt es viele persönliche Beziehungen zwischen Menschen und Kisori, weil Zigtausende Kisori im Solsystem bei der Verteidigung halfen. Inzwischen ist Kisor dabei, Thoris als Haupthandelspartner des Solsystems abzulösen. Thoris ist zwar viel näher, aber seit der Belagerung und der Hilfe Kisors orientiert sich das Solsystem stark an kisorischer Technologie. # Die Information über Solberg 86 III kommt tatsächlich von Kisor. Das System war beschrieben in der Dokumentation zu einem automatischen Asteroiden-Miner. Dhatu Metals hatte von der Siko-Gilde den Miner gekauft. Im Anhang des Benutzerhandbuchs der Miner-KI befand sich eine Datenbank von autonomen Systemen und darunter waren nicht nur kommerzielle kisorische Modelle, sondern auch ein Überblick über exotische KI-Systeme. Das war vermutlich ein Fehler beim Zusammenstellen der Dokumentation. Anscheinend wurden beim Kopiervorgang statt der Quellenangabe aus Versehen die gesamten 23 Terabyte des Quellmaterials mit in den Anhang kopiert. # Ein Mitarbeiter von Dhatu Metals entdeckte die Datenbank während der Inbetriebnahme des Miners. Über die Technologietransferstelle der Bengaluru-Universität und ein Projektmeeting des Sonderforschungsbereichs 'Isolierte Autonome Systeme' der Shankara-Forschungsgesellschaft gelangten die Daten an das Zentrum für interstellare Geschichte der Saurashtra Universität Rajkot/Gujarat. Professorin Khatia Buniatishvili stellte einen Antrag für eine Feldexpedition im Rahmen des Forschungsschwerpunkts 'Replikationsinhibitoren' der Shankara-Gesellschaft. # Der Antrag wird genehmigt und so kommt es, dass unter der Leitung der Geschichtsprofessorin Khatia Buniatishvili ein gechartertes Raumschiff im Orbit von Solberg 86 III kreist und versucht, mehr über die dortige Roboterzivilisation herauszufinden. Die Expedition ist gewissermaßen ein Joint-Venture zwischen der Historischen Fakultät der Saurashtra Universität, die sich weit zurückreichende geschichtliche Daten erhofft, und der Shankara-Gesellschaft, die erforschen will, wie die autonomen Systeme von Solberg 86 III so lang stabil bleiben konnten. # Seit fast 30 Millionen Jahren existiert die Roboterzivilisation schon. Genau gesagt seit 29.825.413 irdischen Jahren. Damals starb der letzte ihrer Erbauer. Die Roboter geben bereitwillig Auskunft über den gesamten Zeitverlauf. Dafür wurden sie programmiert. Die Aufgabe der Roboterzivilisation ist es, das Andenken der Erbauer zu bewahren und jedem (auf Anfrage) alle Informationen über die Erbauer zu geben. Dazu gehört ihre Geschichte, ihre Biologie, die Gesellschaft, Philosophie, Kunst, ihre Technologie und Biographien einzelner Erbauer seit Beginn der Aufzeichnungen. Es ist ein gewaltiger Berg an Informationen. Der vollständige Datenabzug einer ganzen Zivilisation. # Besonders die Technologie scheint interessant, bis man herausfindet, dass es im Hightech-Bereich viele Lücken gibt, vor allem bei Dual-Use Technologien, die auch militärisch verwendbar sind. # Die historischen Daten über die Zivilisation der Erbauer sind sehr umfangreich. Man erfährt alles über ihre 80.000-jährige Geschichte und über die Völker in der Umgebung vor 30 Millionen Jahren. # Aber noch interessanter wären genaue Daten zur Geschichte des Sektors im Verlauf der letzten 30 Millionen Jahre, vor allem zur jüngeren Geschichte, also der letzten 100.000 Jahre. Leider beschränken sich die Informationen seit dem Ende der Erbauer auf eine Liste der Besucher im Solberg 86 System. Aus den Beschreibungen kann man Informationen zum umliegenden Sektor ableiten. Die Herkunft der Besucher gibt indirekt Auskunft über die Abfolge der wichtigen Völker im Lauf der Geschichte. Aber die Roboter haben selbst nie das System verlassen, um weitere Daten zu sammeln. # Einige interessante Erkenntnisse gibt es aber doch. Schon vor 150.000 Jahren, und seitdem immer wieder, waren Kelaner unter den Besuchern. Kelaner haben wohl nie eine bedeutende Rolle im lokalen Sektor gespielt. Aber sie waren nach den Angaben anderer Völker irgendwie schon immer da. Jetzt weiß man ungefähr wie lang Kelaner schon zu den raumfahrenden Völkern gehören. # Man sieht auch deutlich die Wellen der kisorischen Zivilisation über die letzten 15.000 Jahre. Die Statistik zeigt Aufstieg und Fall des legendären solemischen Reiches und man erkennt den Beginn des Interianischen Imperiums. Damals besuchten sogar mehrmals Personen, Hände des Imperiums, das Solberg 86 System. Unzählige Völker haben Solberg 86 besucht. Die meisten kann man heute, nach Millionen Jahren, nicht mehr zuordnen. # Im Lauf der Jahrtausende und Jahrmillionen kamen auch immer wieder feindliche Besucher. Neben den üblichen Plünderern, die ohne großen Aufwand abgewehrt wurden, gab es auch viele Versuche die Roboterzivilisation zu vernichten. Mech-Zivilisationen werden von den meisten Völkern nicht gerne gesehen. Alle Völker haben während ihrer technischen Entwicklung mit KI-Ausbrüchen zu kämpfen. Meistens werden diese eingedämmt und die Technologie dann mit Replikationsinhibitoren versehen. Autonome Systeme sollen nur den Betreibern dienen, nicht selbständig agieren und sich auf keinen Fall unkontrolliert ausbreiten. Selbständige Mech-Zivilisationen, wie Solberg 86 III, die ohne ihre biologischen Erbauer weiter existieren, werden als Bedrohung angesehen. Im Lauf der Zeit haben viele lokale Ordnungsmächte, die jeweils herrschenden Imperien und Allianzen von Nachbarvölkern, Solberg 86 angegriffen. Aber alle wurden abgewehrt. # Die Roboter geben auch dazu bereitwillig Auskunft. Es gibt detaillierte Aufzeichnungen von allen Angriffen und in manchen Fällen sieht man riesige Mengen von Kampfeinheiten, die von allen Monden des Solberg 86-Systems aus verborgenen Arsenalen starten. Und man sieht beeindruckende Waffenwirkungen, die leider nicht in der Technologiedatenbank verzeichnet sind. # Die Roboterzivilisation hatte Millionen Jahre Zeit, um Kampfmittel zu bauen. Offensichtlich halten die Roboter ihr Material auch nach so langer Zeit betriebsbereit. Dafür ist sicher eine riesige technische Infrastruktur notwendig. # Auf den ersten Blick erscheint nur der dritte Planet von Robotern 'bewohnt'. Es gibt geringe Bergbauaktivitäten in den Asteroiden und den Monden der Gasplaneten. Aber bei genauerem Hinsehen entdeckt man, dass große Teile des Systems, viele Monde und Asteroiden, ausgebaut sind und unterirdische technische Anlagen besitzen. # Die Roboter haben den Auftrag das Andenken ihrer Erbauer zu bewahren. Sie sorgen mit allen Mitteln dafür, dass das so bleibt. Der Auftrag ändert sich nicht. Sie ändern sich nicht. Es gibt keine Mutation, keine Neuinterpretation des Auftrags, keine Ausbreitung, keine Bedrohung anderer Völker. Zeit spielt für sie keine Rolle. Sie sind da. Sie bleiben da. Sie befolgen weiter ihren 30 Millionen Jahre alten Auftrag. # Über sich selbst geben die Roboter wenig Auskunft. Ihr Informationsauftrag bezieht sich nur auf ihre Erbauer. So bekommen die mitgereisten KI-Forscher nur wenig Substantielles über den langfristigen autonomen Betrieb der Mech-Zivilisation. Die Historiker der Expedition erfahren sehr viel über die Geschichte vor 30 Millionen Jahren, aber wenig über die jüngere Vergangenheit. # Die Expedition liefert interessante Erkenntnisse. Alleine die Tatsache, dass es diese Mech-Zivilisation schon so lang gibt, öffnet vielen Menschen die Augen wie groß und vielfältig das Universum ist. Auf der anderen Seite sind die meisten Informationen nur indirekt (abgeleitet aus den Besucherlisten), unvollständig (im Hightech-Bereich) oder inzwischen irrelevant (wie die ausführliche Geschichte der Erbauer in einer längst vergangenen Epoche). | tags=#Roboter #Geschichte #Zivilisation #Erbe | image=Solberg.jpg | twitter=2681 Entdeckung einer #Roboter-#Zivilisation. Sie bewachen das #Erbe ihrer Erbauer http://jmp1.de/h2681 #SciFi | twitterimage=Solberg-twpost.jpg | facebook=Roboter bewachen ihr Erbe: Seit 30 Millionen Jahren - 2681 Entdeckung einer Roboterzivilisation. Eine Expedition erreicht das System des Sterns Solberg 86. Die Wissenschaftler gehen in einem Orbit um den dritten Planeten. Dort finden sie eine Roboterzivilisation. Die Roboter bewachen ein uraltes Erbe. Die Expedition hatte das System gezielt angeflogen nach Recherchen in einer alten extrasolaren Bibliothek... Weiter: http://jmp1.de/h2681 # 2681 Entdeckung einer uralten Roboterzivilisation # Eine Expedition erreicht das System des Sterns Solberg 86. Die Wissenschaftler gehen in einem Orbit um den dritten Planeten. Dort finden sie, wie erwartet, eine Roboterzivilisation. Die Roboter bewachen ein uraltes Erbe. Die Expedition hatte das System gezielt angeflogen nach Recherchen in einer alten extrasolaren Bibliothek... Weiter: http://jmp1.de/h2681 | facebookimage=Solberg-fbpost.jpg | topic=discovery,ai".Replace("'", "\"");
            var entries = new Bigfile2Yaml().Analyse(data);
            Assert.AreEqual(1, entries.Count);
            var e = entries[0];
            Assert.AreEqual("Solberg", e.Name);
            Assert.AreEqual("2681", e.Year);
            Assert.AreEqual("Entdeckung einer uralten Roboterzivilisation auf dem Planeten Solberg 86 III", e.Title);
            Assert.AreEqual("Eine Expedition erreicht das System des Sterns Solberg 86. Die Wissenschaftler gehen in einem Orbit um den dritten Planeten. Dort finden sie, wie erwartet, eine Roboterzivilisation. Die Roboter bewachen ein uraltes Erbe. Die Expedition hatte das System gezielt angeflogen nach Recherchen in einer alten extrasolaren Bibliothek...", e.Short);
            Assert.AreEqual("", e.Summary);
            Assert.AreEqual("Roboter bewachen ihr Erbe: Seit 30 Millionen Jahren", e.Headline);
            Assert.AreEqual("Solberg.jpg", e.Image);
            Assert.AreEqual("", e.Smallimage);
            Assert.AreEqual("2681 Entdeckung einer Roboterzivilisation. Eine Expedition erreicht das System des Sterns Solberg 86. Die Wissenschaftler gehen in einem Orbit um den dritten Planeten. Dort finden sie eine Roboterzivilisation. Die Roboter bewachen ein uraltes Erbe. Die Expedition hatte das System gezielt angeflogen nach Recherchen in einer alten extrasolaren Bibliothek... Weiter: http://jmp1.de/h2681", e.Post);
            Assert.AreEqual("Solberg-twpost.jpg", e.Postimage);
            Assert.AreEqual("2681 Entdeckung einer #Roboter-#Zivilisation. Sie bewachen das #Erbe ihrer Erbauer http://jmp1.de/h2681 #SciFi", e.Twitter);
            Assert.AreEqual("Solberg-twpost.jpg", e.Twitterimage);
            Assert.AreEqual("2681 Entdeckung einer Roboterzivilisation. Eine Expedition erreicht das System des Sterns Solberg 86. Die Wissenschaftler gehen in einem Orbit um den dritten Planeten. Dort finden sie eine Roboterzivilisation. Die Roboter bewachen ein uraltes Erbe. Die Expedition hatte das System gezielt angeflogen nach Recherchen in einer alten extrasolaren Bibliothek... Weiter: http://jmp1.de/h2681", e.Facebook);
            Assert.AreEqual("Solberg-fbpost.jpg", e.Facebookimage);
            Assert.AreEqual(4, e.Tags.Count);
            Assert.AreEqual(2, e.Topics.Count);
            Assert.AreEqual(21, e.Text.Count);
        }
    }
}
