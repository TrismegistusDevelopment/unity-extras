copy README.md "Packages/trismegistus.unity.core/ReadMe.md" /Y
copy CHANGELOG.md "Packages/trismegistus.unity.core/CHANGELOG.md" /Y
cd Packages/trismegistus.unity.core
npm publish --registry http://upm.trismegistus.tech:4873/ || pause