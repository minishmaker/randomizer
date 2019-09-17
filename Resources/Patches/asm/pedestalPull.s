.equ defaultRequirement, requirementTable+4
.equ pedestalearth, defaultRequirement+4
.equ pedestalfire, pedestalearth+4
.equ pedestalwater, pedestalfire+4
.equ pedestalwind, pedestalwater+4
.thumb
push	{r4,lr}
@check if we already pulled
ldr	r0,=#0x2002D0B
ldrb	r0,[r0]
mov	r1,#0x10
and	r0,r1
cmp	r0,#0
beq	checkconditions

@open the door at the back
ldr	r0,=#0x80F4BDC
ldr	r3,=#0x804AAF8
mov	lr,r3
.short	0xF800
b	elements

@check if requirements are met
checkconditions:
ldr	r4,requirementTable
loop:
ldr	r0,[r4]
cmp	r0,#0
beq	endLoop
mov	lr,r0
.short	0xF800
cmp	r0,#0
beq	elements
add	r4,#4
b	loop
beq	endLoop

endLoop:
ldr	r0,requirementTable
cmp	r0,r4
bne	elements
@run the default requirement
ldr	r0,defaultRequirement
mov	lr,r0
.short	0xF800
cmp	r0,#0
beq	elements

@place the drop object
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
pop	{r4,pc}
.align
.ltorg
requirementTable:
@POIN requirementTable
@POIN defaultRequirement
@POIN pedestalearth
@POIN pedestalfire
@POIN pedestalwater
@POIN pedestalwind
