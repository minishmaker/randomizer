.thumb
@if using the lantern, mark it so already so the combo fix works with either order of items
ldr	r0,=#0x300116C
ldrb	r1,[r0,#3]
cmp	r1,#6
bcs	notLantern
ldrb	r1,[r0]
cmp	r1,#1
bne	notLantern
cmp	r4,#0x0F
bne	notLantern
ldr	r0,=#0x3000BE3
mov	r1,#6
strb	r1,[r0]

@check if using stairs
notLantern:
ldr	r0,=#0x3003F8C
ldrb	r0,[r0]
cmp	r0,#0
bne	cutscene
@check if starting a cutscene
ldr	r0,=#0x300116C
ldrb	r1,[r0,#3]
cmp	r1,#6
bhs	return0
cutscene:
@and if ocarina
cmp	r4,#0x17
bne	notOcarina
ldrb	r0,[r0]
cmp	r0,#1
bne	return0

notOcarina:
@check if starting lantern
ldr	r0,=#0x3000BE3
ldrb	r1,[r0]
cmp	r1,#0
beq	notCombo
@check if using bow or boomerang
cmp	r4,#0x09
blo	notCombo
cmp	r4,#0x0C
bls	return0

notCombo:
mov	r0,r4
ldr	r3,=#0x8077378
mov	lr,r3
.short	0xF800
b	end
return0:
mov	r0,#0
end:
mov	r1,r0
ldr	r3,=#0x80772E1
bx	r3
