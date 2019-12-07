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
            var data = "2052	Personenrechte für Primaten # Als erstes Land beschließt Island, Primaten als Personen anzuerkennen. Einige, speziell benannte Primatenarten werden rechtlich Kleinkindern gleichgestellt. Damit erhalten sie theoretisch die Menschenrechte, vor allem Persönlichkeitsrechte (Leben, körperliche Unversehrtheit) und Freiheitsrechte. Wenn nötig, bekommen sie einen Vormund, der ihre Rechte vertritt. Praktisch müssen ihre Rechte in Einzelfällen vor Gericht erstritten werden. Aber wenn ein Vormund entscheidet, dass ein bestimmter Schimpanse nicht im Käfig gehalten werden soll, dann stehen die Chancen gut, dass ein Gericht dem Antrag stattgibt, basierend auf dem Selbstbestimmungsrecht. # Das Gesetz ist sehr weitreichend. Mit der neuen Einordnung von Primaten werden auch andere Tiere aufgewertet. Einige Arten mit relativ hohen kognitiven Fähigkeiten werden zwar nicht als Personen, aber immerhin als nichtmenschliche Wesen eingeordnet. Darunter sind Oktopus, Elefant, Delfin, einige Wale, Wolf, Hund, und einige Haustiere. Diese Tiere werden nun nicht mehr als 'Sache' betrachtet, die immer einem Menschen gehört, sondern als 'Wesen', einer neuen Klasse rechtlicher Entitäten neben dem Menschen und den 'Sachen'. Damit sind Individuen dieser Arten nicht mehr automatisch das Eigentum von Menschen oder juristischen Personen. # Eine dauerhafte Kommission wird eingerichtet, um über die Auswahl der Arten zu entscheiden. Die Kommission ist besetzt mit Vertretern vieler gesellschaftlicher Gruppen. Sie bewertet die Selbsterkenntnis, soziales Verhalten, Planungsfähigkeiten, die Fähigkeit zu sprechen (durch Laute oder Zeichensprache), Ursache und Wirkung zu erkennen, Innovation, Imitation und Lernverhalten. Das Althing (das isländische Parlament) gibt explizit vor, dass in der Diskussion der Kommission nur neue Erkenntnisse verwendet werden dürfen, die nach der wissenschaftlichen Methode (Beobachtung-Hypothese-Test) gewonnen werden. Explizit ausgeschlossen werden damit traditionelle und kommerzielle Argumente, also 'das war schon immer so', bzw. 'das steht in alten Schriften' oder 'das können wir uns nicht leisten'. Im Jahr 2062 kommen – gegen den Widerstand der Lebensmittelindustrie – auch Nutztiere dazu. Damit wird die Haltung von Schweinen zur Fleischproduktion praktisch illegal. # 'Wie sollen wir mit Wesen umgehen, die sich im Spiegel erkennen, die um Gefährten trauern, die ein Bewusstsein für sich als Individuum haben? Verdienen sie es nicht, dass wir sie so behandeln wie andere, genauso empfindsame Wesen: uns selbst?' - Jane Goodall. # Es war ein langer Weg vom Verbot von Pelzfarmen, Legebatterien und Tierversuchen für Kosmetika am Anfang des 21. Jahrhunderts bis zur ersten vollen Anerkennung als Person durch einen souveränen Staat. Auch vorher gab es schon Einzelfälle in denen Tieren Rechte zugesprochen wurden. In Indien sind Tiere schon lang als 'Wesen' (im Gegensatz zu 'Sachen') geschützt. Auch in Deutschland hatten Tiere Rechte, die allerdings in der Praxis oft hinter kommerziellen Argumenten zurücktreten mussten. In den 20er und 30er Jahren bekam die Bewegung für Tierrechte (NHR, non human rights movement) immer mehr Zulauf. Skandinavien, Neuseeland, Bhutan und Ecuador beschlossen Rechte für Tiere, sowie Ausbeutungs- und Misshandlungsverbote. In den 40er Jahren kamen viele Länder und selbstverwaltete Regionen dazu. # Schließlich setzt sich die Erkenntnis durch, dass die Menschlichkeit keine entweder/oder Entscheidung ist, sondern dass es ein Kontinuum gibt in den kognitiven und sozialen Fähigkeiten, dass Schimpansen genauso intelligent, selbstbewusst und mitfühlend sind wie Menschenkinder, dass wir auch Menschen mit eingeschränkten Fähigkeiten nicht die Menschlichkeit absprechen und dass es dem Menschen nicht wirklich zum Schaden gereicht, wenn er Mitwesen genauso schützt, wie schützenswerte Mitmenschen. # Nach Island folgen schnell andere skandinavische und dann europäische Staaten mit der vollen Anerkennung von Personenrechten für Primaten. # In den folgenden 20 Jahren schließen sich weitere 50 Staaten der Erde an. Bezogen auf die (menschliche) Bevölkerung sind das allerdings nur 15%. # Orang-Utans werden die Menschenrechte posthum zuerkannt. | tags=#Menschenrechte #Primaten #Personen #Tiere #Delfine #Wale | image=Primaten.jpg | twitter=2052 Personenrechte für Primaten http://www.galactic-developments.de/data2.html?hilite=2052 #scifi #Menschenrechte #Primaten #Personen #Tiere #Delfine #Wale | twitterimage=Primaten-snpost.jpg | facebook=Unsere Brüder und Schwestern: Mitwesen der Menschen - 2052 Personenrechte für Primaten. Immer mehr Staaten erkennen die großen Menschenaffen als Personen an und gewähren ihnen Menschenrechte oder vergleichbare Rechte... Weiter: http://jmp1.de/h2052 (Bitte liken und teilen) # 2052 Personenrechte für Primaten # Immer mehr Staaten erkennen die großen Menschenaffen als Personen an und gewähren ihnen Menschenrechte oder vergleichbare Rechte... Weiter: http://jmp1.de/h2052 | facebookimage=Primaten-snpost.jpg | topic=ecology".Replace("'", "\"");
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
    }
}
