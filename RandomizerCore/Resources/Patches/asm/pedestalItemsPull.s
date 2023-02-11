.equ pedestalfire, pedestalearth+4
.equ pedestalwater, pedestalfire+4
.equ pedestalwind, pedestalwater+4
.thumb
push	{r4-r7,lr}
@set pulled flag
ldr	r0,=#0x2002D0B
ldrb	r1,[r0]
mov	r2,#0x10
orr	r1,r2
strb	r1,[r0]
ldr	r0,=#0x80F4BDC
ldr	r3,=#0x804AAF8
mov	lr,r3
.short	0xF800

@check how many elements we have
getcount:
mov	r4,#0x40
mov	r5,#0
ldr	r6,=#0x807C4A8
loop:
mov	r0,r4
mov	lr,r6
.short	0xF800
cmp	r0,#0
beq	nope
add	r5,#1
nope:
add	r4,#1
cmp	r4,#0x44
bne	loop

two:
@check element count
cmp	r5,#2
blo	elements
@check if we pulled already
ldr	r0,=#0x2002EA4
ldr	r1,=#31
ldr	r3,=#0x801D5E0	@vanilla flag check routine
mov	lr,r3
.short	0xF800
cmp	r0,#0
bne	three
@spawn pulling object
ldr	r0,=#0x80F4B9C
ldr	r3,=#0x804AAF8
mov	lr,r3
.short	0xF800

three:
@check element count
cmp	r5,#3
blo	elements
@check if we pulled already
ldr	r0,=#0x2002EA4
ldr	r1,=#32
ldr	r3,=#0x801D5E0	@vanilla flag check routine
mov	lr,r3
.short	0xF800
cmp	r0,#0
bne	four
@spawn pulling object
ldr	r0,=#0x80F4B9C
ldr	r3,=#0x804AAF8
mov	lr,r3
.short	0xF800

four:
@check element count
cmp	r5,#4
blo	elements
@check if we pulled already
ldr	r0,=#0x2002EA4
ldr	r1,=#33
ldr	r3,=#0x801D5E0	@vanilla flag check routine
mov	lr,r3
.short	0xF800
cmp	r0,#0
bne	elements
@spawn pulling object
ldr	r0,=#0x80F4B9C
ldr	r3,=#0x804AAF8
mov	lr,r3
.short	0xF800

elements:
@place the elements we have
ldr	r4,=#0x2002B42
ldrb	r4,[r4]

checkearth:
mov	r0,#0x01
and	r0,r4
cmp	r0,#0
beq	checkfire
ldr	r0,pedestalearth
ldr	r3,=#0x804AAF8
mov	lr,r3
.short	0xF800

checkfire:
mov	r0,#0x04
and	r0,r4
cmp	r0,#0
beq	checkwater
ldr	r0,pedestalfire
ldr	r3,=#0x804AAF8
mov	lr,r3
.short	0xF800

checkwater:
mov	r0,#0x10
and	r0,r4
cmp	r0,#0
beq	checkwind
ldr	r0,pedestalwater
ldr	r3,=#0x804AAF8
mov	lr,r3
.short	0xF800

checkwind:
mov	r0,#0x40
and	r0,r4
cmp	r0,#0
beq	end
ldr	r0,pedestalwind
ldr	r3,=#0x804AAF8
mov	lr,r3
.short	0xF800

end:
pop	{r4-r7,pc}
.align
.ltorg
pedestalearth:
@POIN pedestalearth
@POIN pedestalfire
@POIN pedestalwater
@POIN pedestalwind
