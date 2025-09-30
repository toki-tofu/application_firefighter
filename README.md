le projet est un projet visual studio il faut donc installer l'entièreté des packages pour pouvoir lancer l'éxecutable ; ce projet a été réalisé en groupe de 3 

# 🚒 Projet Fiche Mission SDIS67

## 📖 Description
Ce projet est une application Windows Forms en C# permettant aux pompiers de consulter et gérer les missions stockées dans une base de données SQLite. Chaque mission peut être consultée, marquée comme terminée, enrichie d’un compte rendu, et exportée sous forme de fichier PDF bien formaté.

---

## ✨ Fonctionnalités principales

- **Chargement des missions** depuis la base SQLite `SDIS67.db`.
- **Affichage des missions** dans un panneau défilant (`Panel` avec `AutoScroll`).
- **Marquer une mission comme terminée** :
  - Ajoute automatiquement la date et l’heure de retour.
  - Sauvegarde un **compte rendu** saisi dans une zone de texte (`TextBox`).
- **Génération de PDF** :
  - Contient les informations de la mission.
  - Liste les **pompiers mobilisés** (nom, prénom, sexe, portable, grade).
  - Liste les **engins mobilisés** (numéro, type, caserne).
  - Intègre le **compte rendu** saisi par l’utilisateur.
- **Choix de l’emplacement du PDF** grâce à un `SaveFileDialog`.
- **Ouverture automatique du PDF** généré.

---

## 🛠️ Technologies utilisées

- **C# WinForms** (interface graphique)
- **SQLite** (gestion des données)
- **QuestPDF** (génération des fichiers PDF)

---

## 📂 Structure des tables utilisées

- **Mission** : id, dateHeureDepart, motifAppel, adresse, cp, ville, terminee, dateHeureRetour, compteRendu
- **Mobiliser** : idMission, matriculePompier
- **Pompier** : matricule, nom, prenom, sexe, portable, codeGrade
- **PartirAvec** : idMission, numeroEngin, codeTypeEngin, idCaserne

---

## 🚀 Installation et exécution

1. Cloner le projet ou le télécharger.
2. Ouvrir la solution dans Visual Studio (version compatible .NET Framework < 8 si nécessaire).
3. Installer les dépendances NuGet :
   - `System.Data.SQLite`
   - `QuestPDF`
4. Vérifier que la base de données `SDIS67.db` est présente dans le répertoire de sortie (`bin/Debug` ou `bin/Release`), la base de données d'origine a été supprimé pour des raisons de confiddentialité .
5. Lancer l’application.

---

## 📘 Utilisation

- **Au lancement** : toutes les missions s’affichent.
- **Bouton "Terminer"** : termine la mission sélectionnée, enregistre la date de retour et le compte rendu.
- **Bouton "Générer PDF"** : ouvre une boîte de dialogue pour enregistrer le fichier PDF, qui est ensuite généré et ouvert automatiquement.


## ✅ Améliorations possibles
- Ajouter un filtre par mission (terminée / en cours).
- Export de plusieurs missions en un seul fichier PDF.
- Interface plus moderne (icônes, couleurs, etc.).
- Historisation des PDF générés.

👨‍💻 Développé pour le SDIS67 – gestion des missions et génération de rapports.
