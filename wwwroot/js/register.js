export class CountrySelector {
    constructor(allCountryCodes) {
        this.allCountryCodes = allCountryCodes;
        this.initialize();
    }

    initialize() {
        $('#countrySelect').on('change', () => {
            const countryCode = $('#countrySelect').val();
            const countryData = this.allCountryCodes[countryCode];
            if (countryData) {
                this.updateCountryData(countryData.name, countryData.image);
            }
        });
        this.setUserCountry();
    }

    setUserCountry() {
        const userCountryCode = navigator.language.split("-")[0];
        if (userCountryCode) {
            const userCountryCodeUppercase = userCountryCode.toUpperCase();
            const userCountryData = this.allCountryCodes[userCountryCodeUppercase];
            console.log(userCountryData);
            if (userCountryData) {
                $('#countrySelect').val(userCountryCodeUppercase);
                this.updateCountryData(userCountryData.name, userCountryData.image);
            }
        }
    }

    updateCountryData(name, url) {
        this.updateCountryName(name);
        this.updateCountryFlag(url, name);
    }

    updateCountryFlag(url, name) {
        $('#countryFlag').attr('src', url);
        $('#countryFlag').attr('alt', name);
    }

    updateCountryName(name) {
        $('#Input_Country').val(name);
    }
}
