﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using intprogvize.Models;
using intprogvize.ViewModel;

namespace intprogvize.Controllers
{
    public class ServisController : ApiController
    {

        DB03Entities db = new DB03Entities();
        SonucModel sonuc = new SonucModel();
        #region Öğrenci

        [HttpGet]
        [Route("api/ogrencilistesi")]
        public List<OgrenciModel> OgrenciListe()
        {
            List<OgrenciModel> liste = db.Ogrenci.Select(x => new OgrenciModel()
            {
                ogrId = x.ogrId,
                ogrNo = x.ogrNo,
                ogrAdsoyad = x.ogrAdsoyad,
                ogrKullaniciAdi = x.ogrKullaniciAdi,
                ogrSifre = x.ogrSifre,
                ogrDersSayisi = x.Kayit.Count()
            }).ToList();
            return liste;
        }

        [HttpGet]
        [Route("api/ogrencibyno/{ogr}")]
        public OgrenciModel OgrenciById(string ogrNo)
        {
            OgrenciModel kayit = db.Ogrenci.Where(s => s.ogrNo == ogrNo).Select(x => new OgrenciModel()
            {
                ogrId = x.ogrId,
                ogrNo = x.ogrNo,
                ogrAdsoyad = x.ogrAdsoyad,
                ogrKullaniciAdi = x.ogrKullaniciAdi,
                ogrSifre = x.ogrSifre,
                ogrDersSayisi = x.Kayit.Count()
            }).SingleOrDefault();
            return kayit;
        }

        [HttpPost]
        [Route("api/ogrenciekle")]
        public SonucModel OgrenciEkle(OgrenciModel model)
        {
            if (db.Ogrenci.Count(c => c.ogrNo == model.ogrNo) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen Öğrenci Numarası Kayıtlıdır!";
                return sonuc;
            }

            Ogrenci yeni = new Ogrenci();
            yeni.ogrId = model.ogrId;
            yeni.ogrNo = model.ogrNo;
            yeni.ogrAdsoyad = model.ogrAdsoyad;
            yeni.ogrKullaniciAdi = model.ogrKullaniciAdi;
            yeni.ogrSifre = model.ogrSifre;
            db.Ogrenci.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Öğrenci Eklendi";
            return sonuc;
        }
        [HttpPut]
        [Route("api/ogrenciduzenle")]
        public SonucModel OgrenciDuzenle(OgrenciModel model)
        {
            Ogrenci kayit = db.Ogrenci.Where(s => s.ogrId == model.ogrId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunmadı!";
                return sonuc;
            }

            kayit.ogrNo = model.ogrNo;
            kayit.ogrAdsoyad = model.ogrAdsoyad;
            kayit.ogrKullaniciAdi = model.ogrKullaniciAdi;
            kayit.ogrSifre = model.ogrSifre;

            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Öğrenci Düzenlendi";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/ogrencisil/{ogrId}")]
        public SonucModel OgrenciSil(string ogrId)
        {
            Ogrenci kayit = db.Ogrenci.Where(s => s.ogrId == ogrId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunmadı!";
                return sonuc;
            }

            if (db.Kayit.Count(c => c.kayitOgrId == ogrId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Derse Kayıtlı Öğrenci Silinemez!";
                return sonuc;
            }


            db.Ogrenci.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Öğrenci Silndi";
            return sonuc;
        }

        #endregion

        #region Öğretmen
        [HttpGet]
        [Route("api/ogretmenlistesi")]
        public List<OgretmenModel> OgretmenListe()
        {
            List<OgretmenModel> liste = db.Ogretmen.Select(x => new OgretmenModel()
            {
                ogId = x.ogId,
                ogAdsoyad = x.ogAdsoyad,
            }).ToList();
            return liste;
        }

        #endregion

        #region Sınav
        [HttpGet]
        [Route("api/sinavlistesi")]
        public List<DersModel> DersListe()
        {
            List<DersModel> liste = db.Ders.Select(x => new DersModel()
            {
                dersId = x.dersId,
                dersAdi = x.dersAdi,
                dersKodu = x.dersKodu,
                DersOgrSayisi = x.Kayit.Count()
            }).ToList();
            return liste;
        }
        [HttpGet]
        [Route("api/sinavbyid/{sinavId}")]
        public DersModel DersById(string dersId)
        {
            DersModel kayit = db.Ders.Where(s => s.dersId == dersId).Select(x => new DersModel()
            {
                dersId = x.dersId,
                dersAdi = x.dersAdi,
                dersKodu = x.dersKodu,
                DersOgrSayisi = x.Kayit.Count()
            }).SingleOrDefault();
            return kayit;
        }

        [HttpPost]
        [Route("api/sinavekle")]
        public SonucModel DersEkle(DersModel model)
        {
            if (db.Ders.Count(c => c.dersKodu == model.dersKodu) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen Sınav Zaten Kayıtlıdır!";
                return sonuc;
            }

            Ders yeni = new Ders();
            yeni.dersId = Guid.NewGuid().ToString();
            yeni.dersKodu = model.dersKodu;
            yeni.dersAdi = model.dersAdi;
            db.Ders.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Sınav Eklendi";

            return sonuc;
        }
        [HttpPut]
        [Route("api/sinavduzenle")]
        public SonucModel DersDuzenle(DersModel model)
        {
            Ders kayit = db.Ders.Where(s => s.dersId == model.dersId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }

            kayit.dersKodu = model.dersKodu;
            kayit.dersAdi = model.dersAdi;

            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Sınav Kaydedildi";

            return sonuc;
        }

        [HttpDelete]
        [Route("api/sinavsil/{dersId}")]
        public SonucModel DersSil(string dersId)
        {
            Ders kayit = db.Ders.Where(s => s.dersId == dersId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }

            if (db.Kayit.Count(c => c.kayitDersId == dersId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Öğrenci Kayıtlı Sınavlar Silinemez!";
                return sonuc;
            }

            db.Ders.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Sınav Silindi";

            return sonuc;
        }
        #endregion

        #region Kayıt

        [HttpGet]
        [Route("api/ogrencisinavlistesi/{ogrId}")]
        public List<KayitModel> OgrenciDersListe(string ogrId)
        {
            List<KayitModel> liste = db.Kayit.Where(s => s.kayitOgrId == ogrId).Select(x => new KayitModel()
            {
                kayitId = x.kayitId,
                kayitDersId = x.kayitDersId,
                kayitOgrId = x.kayitOgrId
            }).ToList();

            foreach (var kayit in liste)
            {
                kayit.ogrBilgi = OgrenciById(kayit.kayitOgrId);
                kayit.dersBilgi = DersById(kayit.kayitDersId);

            }
            return liste;

        }
        [HttpGet]
        [Route("api/sinavogrencilistesi/{dersId}")]
        public List<KayitModel> DerOgrencisListe(string dersId)
        {
            List<KayitModel> liste = db.Kayit.Where(s => s.kayitDersId == dersId).Select(x => new KayitModel()
            {
                kayitId = x.kayitId,
                kayitDersId = x.kayitDersId,
                kayitOgrId = x.kayitOgrId
            }).ToList();

            foreach (var kayit in liste)
            {
                kayit.ogrBilgi = OgrenciById(kayit.kayitOgrId);
                kayit.dersBilgi = DersById(kayit.kayitDersId);

            }
            return liste;

        }
        #endregion

        #region Kayıt
        [HttpPost]
        [Route("api/kayitekle")]
        public SonucModel KayitEkle(KayitModel model)
        {
            if (db.Kayit.Count(c => c.kayitDersId == model.kayitDersId & c.kayitOgrId == model.kayitOgrId) > 0)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Öğrencinin Sınav Kaydı Önceden Yapılmıştır!";
                return sonuc;
            }

            Kayit yeni = new Kayit();
            yeni.kayitId = Guid.NewGuid().ToString();
            yeni.kayitOgrId = model.kayitOgrId;
            yeni.kayitDersId = model.kayitDersId;
            db.Kayit.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Kayıt Eklendi";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/kayitsil/{kayitId}")]
        public SonucModel KayitSil(string kayitId)
        {
            Kayit kayit = db.Kayit.Where(s => s.kayitId == kayitId).SingleOrDefault();

            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }


            db.Kayit.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Kayıt Silindi";

            return sonuc;
        }
        #endregion
    }
}
