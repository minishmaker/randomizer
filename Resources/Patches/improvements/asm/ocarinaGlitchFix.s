.thumb
@check if starting a cutscene
ldr	r0,=#0x300116C
ldrb	r1,[r0,#3]
cmp	r1,#6
bhs	end
@and if ocarina
cmp	r4,#0x17
bne	notOcarina
ldrb	r0,[r0]
cmp	r0,#1
bne	end

notOcarina:
@check if starting lantern
ldr	r0,=#0x3000BE3
ldrb	r1,[r0]
cmp	r1,#0
beq	noLantern
@check if using bow or boomerang
cmp	r4,#0x0C
bhi	noLantern
cmp	r4,#0x0B
blo	noLantern
b	end

noLantern:
mov	r0,r4
ldr	r3,=#0x8077378
mov	lr,r3
.short	0xF800
end:
mov	r1,r0
ldr	r3,=#0x80772E1
bx	r3
